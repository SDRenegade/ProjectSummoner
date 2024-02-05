using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Gust")]
public class GustSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Gust(terraAttack, this);
    }
}

public class Gust : TerraMoveBase
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    public Gust(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += CheckForFlyingOpponent;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= CheckForFlyingOpponent;
    }

    private void CheckForFlyingOpponent(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetTerraAttack().GetMove())
            return;
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition().GetBattlePositionState() != BattlePositionState.FLYING)
            return;

        eventArgs.GetDirectAttackLog().SetDamage((int)(eventArgs.GetDirectAttackLog().GetDamage() * DAMAGE_MULTIPLIER));
    }
}