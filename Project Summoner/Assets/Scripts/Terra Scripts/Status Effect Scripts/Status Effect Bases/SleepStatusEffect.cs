using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Sleep")]
public class SleepStatusEffect : StatusEffectBase
{
    public SleepStatusEffect()
    {
        statusName = "Sleep";
        description = "Inflicted Terra are unable to attack until they wake up";
    }

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new SleepStatusEffectBattleAction(terra);
    }
}

public class SleepStatusEffectBattleAction : BattleAction
{
    private static readonly int MIN_TURNS_SLEEP = 2;
    private static readonly int MAX_TURNS_SLEEP = 5;

    private Terra terra;
    private int numTurnsSleep;
    private int turnCounter;

    public SleepStatusEffectBattleAction(Terra terra)
    {
        this.terra = terra;
        numTurnsSleep = Random.Range(MIN_TURNS_SLEEP, MAX_TURNS_SLEEP + 1);
        turnCounter = 0;
    }

    public void AddBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration += SleepActive;
    }

    public void RemoveBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration -= SleepActive;
    }

    private void SleepActive(object sender, TerraAttackDeclarationEventArgs terraAttackDeclaration)
    {
        if (terraAttackDeclaration.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        turnCounter++;
        if (turnCounter <= numTurnsSleep) {
            terraAttackDeclaration.GetTerraAttack().SetIsCanceled(true);
            Debug.Log(BattleDialog.SleepProkedMsg(terra));
            terraAttackDeclaration.GetBattleSystem().UpdateTerraStatusBars();
        }
        else {
            terra.SetStatusEffect(null);
            terraAttackDeclaration.GetBattleSystem().OnTerraAttackDeclaration -= SleepActive;
            Debug.Log(BattleDialog.TerraWokeUpMsg(terra));
        }
    }
}

