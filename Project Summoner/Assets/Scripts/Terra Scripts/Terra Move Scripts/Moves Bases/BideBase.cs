using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bide")]
public class BideBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new BideAction(terraAttack);
    }
}

public class BideAction : TerraMoveAction
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    private TerraAttack terraAttack;
    private int totalDamageRecieved;
    private int turnCounter;

    public BideAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        totalDamageRecieved = 0;
        turnCounter = 0;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnTerraDamageByTerra += AccumulateDamage;
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
        battleSystem.OnAttackMissed += AttackMissedAction;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnTerraDamageByTerra -= AccumulateDamage;
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnTerraDamageByTerra -= UnleashDamage;
        battleSystem.OnAttackMissed -= AttackMissedAction;
    }

    private void QueueNextAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        turnCounter++;
        eventArgs.SetSkipActionSelection(true);

        if(turnCounter == 2) {
            eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueNextAttack;
            eventArgs.GetBattleSystem().OnTerraDamageByTerra += UnleashDamage;
        }
    }

    private void AccumulateDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraAttack.GetAttackerPosition())
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        totalDamageRecieved += (int)eventArgs.GetDirectAttackLog().GetDamage();
    }

    private void UnleashDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        eventArgs.GetDirectAttackLog().SetDamage((int)(totalDamageRecieved * DAMAGE_MULTIPLIER));

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}