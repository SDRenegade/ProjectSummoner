using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Freeze")]
public class FreezeStatusEffect : StatusEffectBase
{
    public FreezeStatusEffect()
    {
        statusName = "Freeze";
        description = "Inflicted Terra are unable to attack until they have thawed";
    }

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new FreezeStatusEffectBattleAction(terra);
    }
}

public class FreezeStatusEffectBattleAction : BattleAction
{
    private static readonly float THAW_CHANCE = 0.25f;

    private Terra terra;

    public FreezeStatusEffectBattleAction(Terra terra)
    {
        this.terra = terra;
    }

    public void AddBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration += FreezeActive;
    }

    public void RemoveBattleAction(BattleSystem battleSystem)
    {
        battleSystem.OnTerraAttackDeclaration -= FreezeActive;
    }

    private void FreezeActive(object sender, TerraAttackDeclarationEventArgs terraAttackDeclaration)
    {
        if (terraAttackDeclaration.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        bool hasThawed = THAW_CHANCE >= Random.Range(0f, 1f);
        if (hasThawed) {
            terra.SetStatusEffect(null);
            terraAttackDeclaration.GetBattleSystem().OnTerraAttackDeclaration -= FreezeActive;
            Debug.Log(BattleDialog.TerraThawedMsg(terra));
        }
        else {
            terraAttackDeclaration.GetTerraAttack().SetIsCanceled(true);
            Debug.Log(BattleDialog.FreezeProkedMsg(terra));
            terraAttackDeclaration.GetBattleSystem().UpdateTerraStatusBars();
        }
    }
}
