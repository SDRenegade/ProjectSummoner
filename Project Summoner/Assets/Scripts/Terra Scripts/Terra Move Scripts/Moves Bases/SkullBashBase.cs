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
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnDirectAttack -= ChargeAction;
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
        attackerPosition.SetDefenceStage(StatStagesExtension.IncrementStage(attackerPosition.GetDefenceStage()));
        Debug.Log(BattleDialog.StatStageIncrementMsg(attackerPosition.GetTerra(), attackerPosition.GetDefenceStage(), 1));

        eventArgs.GetBattleSystem().OnDirectAttack -= ChargeAction;
    }


}