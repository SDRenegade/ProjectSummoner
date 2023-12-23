using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Paralysis")]
public class ParalysisStatusEffect : StatusEffectBase
{
    public ParalysisStatusEffect()
    {
        statusName = "Paralysis";
        description = "Inflicted Terra have a chance of being unable to attack each turn";
    }

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new ParalysisStatusEffectBattleAction(terra);
    }
}

public class ParalysisStatusEffectBattleAction : BattleAction
{
    private static readonly float PARALYSIS_CHANCE = 0.50f;

    private Terra terra;

    public ParalysisStatusEffectBattleAction(Terra terra)
    {
        this.terra = terra;
    }

    public void AddBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration += ParalysisActive;
    }

    public void RemoveBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration -= ParalysisActive;
    }

    private void ParalysisActive(object sender, TerraAttackDeclarationEventArgs terraAttackDeclaration)
    {
        if (terraAttackDeclaration.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        bool isParalyzed = PARALYSIS_CHANCE >= Random.Range(0f, 1f);
        if (isParalyzed) {
            terraAttackDeclaration.GetTerraAttack().SetIsCanceled(true);
            Debug.Log(BattleDialog.ParalysisProkedMsg(terra));
            terraAttackDeclaration.GetBattleSystem().UpdateTerraStatusBars();
        }
    }
}
