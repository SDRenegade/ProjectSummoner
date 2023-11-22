using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurnStatusEffect : StatusEffect
{
    private static readonly float PERCENT_MAX_HEALTH_BURN = 1/8f;

    public BurnStatusEffect(Terra terra) : base(terra) {}

    public override void AddBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnEndTurn += BurnActive;
        //Add AttackReductionActive
    }

    public override void RemoveBattleEvent(BattleSystem battleSystem)
    {
        battleSystem.OnEndTurn -= BurnActive;
        //Remove AttackReductionActive
    }

    private void BurnActive(object sender, BattleEventArgs battlesArgs)
    {
        int burnDamage = (int)(terra.GetMaxHP() * PERCENT_MAX_HEALTH_BURN);
        terra.TakeDamage(burnDamage);
        Debug.Log(BattleDialog.BurnProkedMsg(terra, burnDamage));
        battlesArgs.GetBattleSystem().UpdateTerraStatusBars();
    }

    private void AttackReductionActive(object sender, TerraAttackTerraEventArgs terraAttackTerraEventArgs)
    {
        //Reduce terra attack during damage step
    }
}
