using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Skull Bash")]
public class SkullBashBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SkullBashAction(terraAttack);
    }
}

public class SkullBashAction : TerraMoveAction
{
    private static readonly int DEF_MODIFICATION = 1;

    private TerraAttack terraAttack;

    public SkullBashAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
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

        eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueNextAttack;
    }

    private void ChargeAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetMove() != terraAttack.GetMove())
            return;

        TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();

        eventArgs.GetDirectAttackParams().SetDamageStepCanceled(true);

        //*** Stat Change Event ***
        StatChangeEventArgs statChangeEventArgs = eventArgs.GetBattleSystem().InvokeOnStatChange(attackerPosition, Stats.DEF, DEF_MODIFICATION);

        if (!statChangeEventArgs.IsCanceled()) {
            attackerPosition.SetStatStage(Stats.DEF, StatStagesExtension.ChangeStatStage(attackerPosition.GetStatStage(Stats.DEF), statChangeEventArgs.GetModification()));
            Debug.Log(BattleDialog.StatStageChangeMsg(attackerPosition.GetTerra(), Stats.DEF, attackerPosition.GetStatStage(Stats.DEF), statChangeEventArgs.GetModification()));
        }

        eventArgs.GetBattleSystem().OnDirectAttack -= ChargeAction;
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}