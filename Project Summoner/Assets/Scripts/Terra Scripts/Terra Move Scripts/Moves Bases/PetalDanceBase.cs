using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Petal Dance")]
public class PetalDanceBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new PetalDanceAction(terraAttack);
    }
}

public class PetalDanceAction : TerraMoveAction
{
    private static readonly int MIN_TURN_DURATION = 2;
    private static readonly int MAX_TURN_DURATION = 3;

    private TerraAttack terraAttack;
    private int turnDuration;
    private int turnCounter;

    public PetalDanceAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 1;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (turnCounter < turnDuration)
            return;

        if (battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Confusion")))
            Debug.Log(BattleDialog.ConfusionInflictedMsg(terraAttack.GetAttackerPosition().GetTerra()));
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);

        RemoveBattleActions(battleSystem);
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
        battleSystem.OnEndOfTurn += EndOfTurnCounterIncrement;
        battleSystem.OnAttackMissed += AttackMissedAction;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnEndOfTurn -= EndOfTurnCounterIncrement;
        battleSystem.OnAttackMissed -= AttackMissedAction;
    }

    private void QueueNextAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        eventArgs.SetSkipActionSelection(true);
    }

    private void EndOfTurnCounterIncrement(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
    }

    private void AttackMissedAction(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraAttack.GetAttackerPosition())
            return;

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}