using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Sleep")]
public class SleepStatusEffect : StatusEffectBase
{
    public SleepStatusEffect() {}

    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new SleepStatusEffectAction(terraBattlePosition);
    }
}

public class SleepStatusEffectAction : BattleAction
{
    private static readonly int MIN_TURNS_SLEEP = 2;
    private static readonly int MAX_TURNS_SLEEP = 5;

    private TerraBattlePosition terraBattlePosition;
    private int numTurnsSleep;
    private int turnCounter;

    public SleepStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
        numTurnsSleep = Random.Range(MIN_TURNS_SLEEP, MAX_TURNS_SLEEP + 1);
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += SleepActive;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= SleepActive;
    }

    private void SleepActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        turnCounter++;
        if (turnCounter <= numTurnsSleep) {
            Debug.Log(BattleDialog.SleepProkedMsg(terraBattlePosition.GetTerra()));
            eventArgs.GetTerraAttack().SetCanceled(true);
        }
        else {
            Debug.Log(BattleDialog.TerraWokeUpMsg(terraBattlePosition.GetTerra()));
            terraBattlePosition.GetTerra().SetStatusEffect(null);
            RemoveBattleActions(eventArgs.GetBattleSystem());
        }
    }
}

