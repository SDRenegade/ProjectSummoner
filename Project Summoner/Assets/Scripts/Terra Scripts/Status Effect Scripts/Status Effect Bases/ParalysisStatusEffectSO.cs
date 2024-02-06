using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Paralysis")]
public class ParalysisStatusEffectSO : StatusEffectSO
{
    public override StatusEffectBase CreateStatusEffectInstance()
    {
        return new ParalysisStatusEffect(this);
    }
}

public class ParalysisStatusEffect : StatusEffectBase
{
    private static readonly float PARALYSIS_CHANCE = 0.5f;

    private TerraBattlePosition terraBattlePosition;

    public ParalysisStatusEffect(StatusEffectSO statusEffectSO) : base(statusEffectSO) {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnAttackDeclaration += ParalysisActive;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= ParalysisActive;
    }

    private void ParalysisActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        if (PARALYSIS_CHANCE >= Random.Range(0f, 1f)) {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.ParalysisProkedMsg(terraBattlePosition.GetTerra()));
        }
    }
}
