using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParalysisStatusEffect : StatusEffect
{
    //Chance out of 100
    private static readonly int PARALYSIS_CHANCE = 50;

    public ParalysisStatusEffect(Terra terra) : base(terra) { }

    public override void AddBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra += ParalysisActive;
    }

    public override void RemoveBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra -= ParalysisActive;
    }

    private void ParalysisActive(object sender, TerraAttackTerraEventArgs terraAttackTerraBattleArgs)
    {
        if (terraAttackTerraBattleArgs.GetTerraAttack().GetAttackingTerraPosition().GetTerra() != terra)
            return;

        bool isParalyzed = PARALYSIS_CHANCE >= Random.Range(0, 100); 
        if(isParalyzed) {
            terraAttackTerraBattleArgs.GetTerraAttack().SetCancelled(true);
            Debug.Log(BattleDialog.ParalysisProkedMsg(terra));
            terraAttackTerraBattleArgs.GetBattleSystem().UpdateTerraStatusBars();
        }
    }
}
