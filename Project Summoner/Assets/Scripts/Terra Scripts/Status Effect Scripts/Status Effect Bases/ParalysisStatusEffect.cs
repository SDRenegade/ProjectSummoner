using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Paralysis")]
public class ParalysisStatusEffect : StatusEffectBase
{
    public ParalysisStatusEffect() {}

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new ParalysisStatusEffectAction(terra);
    }
}

public class ParalysisStatusEffectAction : BattleAction
{
    private static readonly float PARALYSIS_CHANCE = 0.50f;

    private Terra terra;

    public ParalysisStatusEffectAction(Terra terra)
    {
        this.terra = terra;
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
        if (eventArgs.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        bool isParalyzed = PARALYSIS_CHANCE >= Random.Range(0f, 1f);
        if (isParalyzed) {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.ParalysisProkedMsg(terra));
            eventArgs.GetBattleSystem().UpdateTerraStatusBars();
        }
    }
}
