using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepStatusEffect : StatusEffect
{
    private static readonly int MIN_TURNS_SLEEP = 2;
    private static readonly int MAX_TURNS_SLEEP = 5;

    private int numTurnsSleep;
    private int turnCounter;

    public SleepStatusEffect(Terra terra) : base(terra)
    {
        numTurnsSleep = Random.Range(MIN_TURNS_SLEEP, MAX_TURNS_SLEEP + 1);
        turnCounter = 0;
    }

    public override void AddBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra += SleepActive;
    }

    public override void RemoveBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra -= SleepActive;
    }

    private void SleepActive(object sender, TerraAttackTerraEventArgs terraAttackTerraBattleArgs)
    {
        if (terraAttackTerraBattleArgs.GetTerraAttack().GetAttackingTerraPosition().GetTerra() != terra)
            return;

        turnCounter++;
        if (turnCounter <= numTurnsSleep) {
            terraAttackTerraBattleArgs.GetTerraAttack().SetCancelled(true);
            Debug.Log(BattleDialog.SleepProkedMsg(terra));
            terraAttackTerraBattleArgs.GetBattleSystem().UpdateTerraStatusBars();
        }
        else {
            terra.SetStatusEffect(null);
            terraAttackTerraBattleArgs.GetBattleSystem().OnTerraAttackTerra -= SleepActive;
            Debug.Log(BattleDialog.TerraWokeUpMsg(terra));
        }
    }
}
