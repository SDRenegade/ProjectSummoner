using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Burn")]
public class BurnStatusEffect : StatusEffectBase
{
    public BurnStatusEffect()
    {
        statusName = "Burn";
        description = "Inflicted Terra take damage each turn and their attack stat is reduced";
    }

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new BurnStatusEffectBattleAction(terra);
    }
}

class BurnStatusEffectBattleAction : BattleAction
{
    private static readonly float PERCENT_MAX_HEALTH_BURN = 1/8f;
    private static readonly float DAMAGE_REDUCTION = 0.8f;

    private Terra terra;

    public BurnStatusEffectBattleAction(Terra terra)
    {
        this.terra = terra;
    }

    public void AddBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringEndTurnState += BurnActive;
        battleSystem.OnTerraDirectAttack += AttackReductionActive;
    }

    public void RemoveBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringEndTurnState -= BurnActive;
        battleSystem.OnTerraDirectAttack -= AttackReductionActive;
    }

    private void BurnActive(object sender, BattleEventArgs eventArgs)
    {
        int burnDamage = (int)(terra.GetMaxHP() * PERCENT_MAX_HEALTH_BURN);
        terra.TakeDamage(burnDamage);
        Debug.Log(BattleDialog.BurnProkedMsg(terra, burnDamage));
        eventArgs.GetBattleSystem().UpdateTerraStatusBars();
    }

    private void AttackReductionActive(object sender, TerraAttackParamsEventArgs eventArgs)
    {
        eventArgs.GetTerraAttackParmas().SetDamageModifier(eventArgs.GetTerraAttackParmas().GetDamageModifier() * DAMAGE_REDUCTION);
    }
}
