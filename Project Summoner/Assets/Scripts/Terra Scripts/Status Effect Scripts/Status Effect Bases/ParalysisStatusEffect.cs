using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Paralysis")]
public class ParalysisStatusEffect : StatusEffectBase
{
    public ParalysisStatusEffect() {}

    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new ParalysisStatusEffectAction(terraBattlePosition);
    }
}

public class ParalysisStatusEffectAction : BattleAction
{
    private static readonly float PARALYSIS_CHANCE = 0.5f;

    private TerraBattlePosition terraBattlePosition;

    public ParalysisStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += ParalysisActive;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= ParalysisActive;
    }

    private void ParalysisActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        bool isParalyzed = PARALYSIS_CHANCE >= Random.Range(0f, 1f);
        if (isParalyzed) {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.ParalysisProkedMsg(terraBattlePosition.GetTerra()));
        }
    }
}
