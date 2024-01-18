using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Hyper Beam")]
public class HyperBeamBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new HyperBeamAction(terraAttack);
    }
}

public class HyperBeamAction : TerraMoveAction
{
    private TerraAttack terraAttack;
    private bool rechargeTurn;

    public HyperBeamAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        rechargeTurn = false;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
        battleSystem.OnDirectAttack += RechargeAction;
        battleSystem.OnAttackMissed += AttackMissedAction;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnDirectAttack -= RechargeAction;
        battleSystem.OnAttackMissed -= AttackMissedAction;
    }

    private void QueueNextAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        eventArgs.SetSkipActionSelection(true);
        terraAttack.SetPersistent(false);
        rechargeTurn = true;

        eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueNextAttack;
    }

    private void RechargeAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;
        if (rechargeTurn == false)
            return;

        eventArgs.GetDirectAttackParams().SetDamageStepCanceled(true);
        Debug.Log(BattleDialog.HYPER_BEAM_RECHARGE);

        eventArgs.GetBattleSystem().OnDirectAttack -= RechargeAction;
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}