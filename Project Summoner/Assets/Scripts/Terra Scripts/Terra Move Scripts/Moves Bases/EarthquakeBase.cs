using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Earthquake")]
public class EarthquakeBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new EarthquakeAction(terraAttack);
    }
}

public class EarthquakeAction : TerraMoveAction
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    private TerraAttack terraAttack;

    public EarthquakeAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += CheckForDiggingOpponent;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
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