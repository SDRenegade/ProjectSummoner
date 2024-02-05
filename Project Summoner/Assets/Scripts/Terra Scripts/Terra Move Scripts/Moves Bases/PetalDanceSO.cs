using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Petal Dance")]
public class PetalDanceSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new PetalDance(terraAttack, this);
    }
}

public class PetalDance : TerraMoveBase
{
    private static readonly int MIN_TURN_DURATION = 2;
    private static readonly int MAX_TURN_DURATION = 3;

    private int turnDuration;
    private int turnCounter;

    public PetalDance(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO)
    {
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 1;
    }

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (turnCounter < turnDuration)
            return;

        if (battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Confusion")))
            Debug.Log(BattleDialog.ConfusionInflictedMsg(terraAttack.GetAttackerPosition().GetTerra()));
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);

        RemoveBattleActions(battleSystem);
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
        battleSystem.OnEndOfTurn += EndOfTurnCounterIncrement;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnEndOfTurn -= EndOfTurnCounterIncrement;
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
}