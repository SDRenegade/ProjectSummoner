using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreezeStatusEffect : StatusEffect
{
    //Chance out of 100
    private static readonly int THAW_CHANCE = 25;

    public FreezeStatusEffect(Terra terra) : base(terra) {}

    public override void AddBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra += FreezeActive;
    }

    public override void RemoveBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackTerra -= FreezeActive;
    }

    private void FreezeActive(object sender, TerraAttackTerraEventArgs terraAttackTerraBattleArgs)
    {
        if (terraAttackTerraBattleArgs.GetTerraAttack().GetAttackingTerraPosition().GetTerra() != terra)
            return;

        bool hasThawed = THAW_CHANCE >= Random.Range(0, 100);
        if (hasThawed) {
            terra.SetStatusEffect(null);
            terraAttackTerraBattleArgs.GetBattleSystem().OnTerraAttackTerra -= FreezeActive;
            Debug.Log(BattleDialog.TerraThawedMsg(terra));
        }
        else {
            terraAttackTerraBattleArgs.GetTerraAttack().SetCancelled(true);
            Debug.Log(BattleDialog.FreezeProkedMsg(terra));
            terraAttackTerraBattleArgs.GetBattleSystem().UpdateTerraStatusBars();
        }
    }
}
