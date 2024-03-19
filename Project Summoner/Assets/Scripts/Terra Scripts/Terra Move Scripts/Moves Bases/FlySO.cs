using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Fly")]
public class FlySO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Fly(terraAttack, this);
    }
}

public class Fly : TerraMoveBase
{
    public Fly(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnDirectAttack += FirstTurnAction;
        battleSystem.OnEnteringActionSelection += QueueSecondAttack;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueSecondAttack;
        battleSystem.OnDirectAttack -= FirstTurnAction;
        battleSystem.OnDirectAttack -= SecondTurnAction;
    }

    private void FirstTurnAction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;

        eventArgs.GetDirectAttackParams().GetAttackerPosition().SetBattlePositionState(BattlePositionState.FLYING);
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
}