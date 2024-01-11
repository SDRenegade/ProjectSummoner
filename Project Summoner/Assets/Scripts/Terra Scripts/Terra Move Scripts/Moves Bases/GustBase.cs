using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Gust")]
public class GustBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new GustAction(terraAttack);
    }
}

public class GustAction : TerraMoveAction
{
    private TerraAttack terraAttack;

    public GustAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += CheckForFlyingOpponent;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= CheckForFlyingOpponent;
    }

    private void CheckForFlyingOpponent(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetTerraAttack().GetMove())
            return;
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition().GetBattlePositionState() != BattlePositionState.FLYING)
            return;

        eventArgs.SetDamage(eventArgs.GetDamage() * 2);
    }
}