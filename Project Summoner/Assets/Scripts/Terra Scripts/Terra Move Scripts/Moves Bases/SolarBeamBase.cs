using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Solar Beam")]
public class SolarBeamBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SolarBeamAction(terraAttack);
    }
}

public class SolarBeamAction : TerraMoveAction
{
    private TerraAttack terraAttack;
    private bool isChargeTurn;

    public SolarBeamAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        isChargeTurn = true;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
        battleSystem.OnDirectAttack += ChargeAction;
        battleSystem.OnAttackMissed += AttackMissedAction;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnDirectAttack -= ChargeAction;
        battleSystem.OnAttackMissed -= AttackMissedAction;
    }

    private void QueueNextAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        eventArgs.SetSkipActionSelection(true);
        terraAttack.SetPersistent(false);
        isChargeTurn = false;

        eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueNextAttack;
    }

    private void ChargeAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;
        if (isChargeTurn == false)
            return;

        eventArgs.GetDirectAttackParams().SetDamageStepCanceled(true);
        Debug.Log(BattleDialog.SOLAR_BEAM_CHARGE);

        eventArgs.GetBattleSystem().OnDirectAttack -= ChargeAction;
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}