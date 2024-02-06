using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Burn")]
public class BurnStatusEffectSO : StatusEffectSO
{
    public override StatusEffectBase CreateStatusEffectInstance()
    {
        return new BurnStatusEffect(this);
    }
}

class BurnStatusEffect : StatusEffectBase
{
    private static readonly float PERCENT_MAX_HEALTH_BURN = 1/8f;
    private static readonly float DAMAGE_REDUCTION = 0.8f;

    private TerraBattlePosition terraBattlePosition;

    public BurnStatusEffect(StatusEffectSO statusEffectSO) : base(statusEffectSO) {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnEndOfTurn += BurnActive;
        battleSystem.OnDirectAttack += AttackReductionActive;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= BurnActive;
        battleSystem.OnDirectAttack -= AttackReductionActive;
    }

    private void BurnActive(object sender, BattleEventArgs eventArgs)
    {
        int burnDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_BURN);
        Debug.Log(BattleDialog.BurnProkedMsg(terraBattlePosition.GetTerra(), burnDamage));
        eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, burnDamage);
    }

    private void AttackReductionActive(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;

        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_REDUCTION);
    }
}
