using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Freeze")]
public class FreezeStatusEffect : StatusEffectBase
{
    public FreezeStatusEffect() {}

    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new FreezeStatusEffectAction(terraBattlePosition);
    }
}

public class FreezeStatusEffectAction : BattleAction
{
    private static readonly float THAW_CHANCE = 0.25f;

    private TerraBattlePosition terraBattlePosition;

    public FreezeStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
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
