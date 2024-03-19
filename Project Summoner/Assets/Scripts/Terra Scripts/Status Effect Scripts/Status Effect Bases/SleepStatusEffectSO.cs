using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Sleep")]
public class SleepStatusEffectSO : StatusEffectSO
{
    public override StatusEffectBase CreateStatusEffectInstance()
    {
        return new SleepStatusEffect(this);
    }
}

public class SleepStatusEffect : StatusEffectBase
{
    private static readonly int MIN_TURNS_SLEEP = 2;
    private static readonly int MAX_TURNS_SLEEP = 5;

    private TerraBattlePosition terraBattlePosition;
    private int numTurnsSleep;
    private int turnCounter;

    public SleepStatusEffect(StatusEffectSO statusEffectSO) : base(statusEffectSO)
    {
        numTurnsSleep = Random.Range(MIN_TURNS_SLEEP, MAX_TURNS_SLEEP + 1);
        turnCounter = 0;
    }

    public override void AddStatusListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnAttackDeclaration += SleepActive;
    }

    public override void RemoveStatusListeners(BattleSystem battleSystem)
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
            RemoveStatusListeners(eventArgs.GetBattleSystem());
        }
    }
}

