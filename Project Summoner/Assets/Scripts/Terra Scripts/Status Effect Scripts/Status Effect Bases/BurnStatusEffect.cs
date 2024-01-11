using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Burn")]
public class BurnStatusEffect : StatusEffectBase
{
    public BurnStatusEffect() {}

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new BurnStatusEffectAction(terra);
    }
}

class BurnStatusEffectAction : BattleAction
{
    private static readonly float PERCENT_MAX_HEALTH_BURN = 1/8f;
    private static readonly float DAMAGE_REDUCTION = 0.8f;

    private Terra terra;

    public BurnStatusEffectAction(Terra terra)
    {
        this.terra = terra;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn += BurnActive;
        battleSystem.OnDirectAttack += AttackReductionActive;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= BurnActive;
        battleSystem.OnDirectAttack -= AttackReductionActive;
    }

    private void BurnActive(object sender, BattleEventArgs eventArgs)
    {
        int burnDamage = (int)(terra.GetMaxHP() * PERCENT_MAX_HEALTH_BURN);
        terra.TakeDamage(burnDamage);
        Debug.Log(BattleDialog.BurnProkedMsg(terra, burnDamage));
        eventArgs.GetBattleSystem().UpdateTerraStatusBars();
    }

    private void AttackReductionActive(object sender, DirectAttackEventArgs eventArgs)
    {
        eventArgs.GetDirectAttackParams().SetDamageModifier(eventArgs.GetDirectAttackParams().GetDamageModifier() * DAMAGE_REDUCTION);
    }
}
