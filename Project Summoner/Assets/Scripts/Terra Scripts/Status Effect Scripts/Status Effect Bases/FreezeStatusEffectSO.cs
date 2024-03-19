using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Freeze")]
public class FreezeStatusEffectSO : StatusEffectSO
{
    public override StatusEffectBase CreateStatusEffectInstance()
    {
        return new FreezeStatusEffect(this);
    }
}

public class FreezeStatusEffect : StatusEffectBase
{
    private static readonly float THAW_CHANCE = 0.25f;

    private TerraBattlePosition terraBattlePosition;

    public FreezeStatusEffect(StatusEffectSO statusEffectSO) : base(statusEffectSO) {}

    public override void AddStatusListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnAttackDeclaration += FreezeActive;
    }

    public override void RemoveStatusListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= FreezeActive;
    }

    private void FreezeActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        bool hasThawed = THAW_CHANCE >= Random.Range(0f, 1f);
        if (hasThawed) {
            terraBattlePosition.GetTerra().SetStatusEffect(null, terraBattlePosition, eventArgs.GetBattleSystem());
            Debug.Log(BattleDialog.TerraThawedMsg(terraBattlePosition.GetTerra()));
        }
        else {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.FreezeProkedMsg(terraBattlePosition.GetTerra()));
        }
    }
}
