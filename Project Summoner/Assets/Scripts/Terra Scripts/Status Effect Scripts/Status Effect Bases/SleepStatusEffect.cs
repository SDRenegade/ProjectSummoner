using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Sleep")]
public class SleepStatusEffect : StatusEffectBase
{
    public SleepStatusEffect() {}

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new SleepStatusEffectAction(terra);
    }
}

public class SleepStatusEffectAction : BattleAction
{
    private static readonly int MIN_TURNS_SLEEP = 2;
    private static readonly int MAX_TURNS_SLEEP = 5;

    private Terra terra;
    private int numTurnsSleep;
    private int turnCounter;

    public SleepStatusEffectAction(Terra terra)
    {
        this.terra = terra;
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
        if (eventArgs.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        turnCounter++;
        if (turnCounter <= numTurnsSleep) {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.SleepProkedMsg(terra));
            eventArgs.GetBattleSystem().UpdateTerraStatusBars();
        }
        else {
            terra.SetStatusEffect(null);
            eventArgs.GetBattleSystem().OnAttackDeclaration -= SleepActive;
            Debug.Log(BattleDialog.TerraWokeUpMsg(terra));
        }
    }
}

