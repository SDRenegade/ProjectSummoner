using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Earthquake")]
public class EarthquakeSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Earthquake(terraAttack, this);
    }
}

public class Earthquake : TerraMoveBase
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    public Earthquake(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += CheckForDiggingOpponent;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= CheckForDiggingOpponent;
    }

    private void CheckForDiggingOpponent(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetTerraAttack().GetMove())
            return;
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition().GetBattlePositionState() != BattlePositionState.DIGGING)
            return;

        eventArgs.GetDirectAttackLog().SetDamage((int)(eventArgs.GetDirectAttackLog().GetDamage() * DAMAGE_MULTIPLIER));
    }
}