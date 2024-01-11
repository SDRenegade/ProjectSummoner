using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Freeze")]
public class FreezeStatusEffect : StatusEffectBase
{
    public FreezeStatusEffect() {}

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new FreezeStatusEffectAction(terra);
    }
}

public class FreezeStatusEffectAction : BattleAction
{
    private static readonly float THAW_CHANCE = 0.25f;

    private Terra terra;

    public FreezeStatusEffectAction(Terra terra)
    {
        this.terra = terra;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += FreezeActive;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= FreezeActive;
    }

    private void FreezeActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition().GetTerra() != terra)
            return;

        bool hasThawed = THAW_CHANCE >= Random.Range(0f, 1f);
        if (hasThawed) {
            terra.SetStatusEffect(null, eventArgs.GetBattleSystem());
            Debug.Log(BattleDialog.TerraThawedMsg(terra));
        }
        else {
            eventArgs.GetTerraAttack().SetCanceled(true);
            Debug.Log(BattleDialog.FreezeProkedMsg(terra));
        }
    }
}
