using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dig")]
public class DigBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DigAction(terraAttack);
    }
}

public class DigAction : TerraMoveAction
{
    private TerraAttack terraAttack;
 
    public DigAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnDirectAttack += FirstTurnAction;
        battleSystem.OnEnteringActionSelection += QueueSecondAttack;
        battleSystem.OnAttackMissed += AttackMissedAction;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueSecondAttack;
        battleSystem.OnDirectAttack -= FirstTurnAction;
        battleSystem.OnDirectAttack -= SecondTurnAction;
        battleSystem.OnAttackMissed -= AttackMissedAction;
    }

    private void FirstTurnAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;

        eventArgs.GetDirectAttackParams().GetAttackerPosition().SetBattlePositionState(BattlePositionState.DIGGING);
        eventArgs.GetDirectAttackParams().SetDamageStepCanceled(true);

        eventArgs.GetBattleSystem().OnDirectAttack -= FirstTurnAction;
    }

    private void QueueSecondAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        eventArgs.SetSkipActionSelection(true);
        terraAttack.SetPersistent(false);

        eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueSecondAttack;
        eventArgs.GetBattleSystem().OnDirectAttack += SecondTurnAction;
    }

    private void SecondTurnAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;

        eventArgs.GetDirectAttackParams().GetAttackerPosition().SetBattlePositionState(BattlePositionState.NORMAL);

        eventArgs.GetBattleSystem().OnDirectAttack -= SecondTurnAction;
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}