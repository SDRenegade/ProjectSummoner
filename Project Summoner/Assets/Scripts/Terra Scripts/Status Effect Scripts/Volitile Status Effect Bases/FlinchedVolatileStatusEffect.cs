using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Flinched")]
public class FlinchedVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new FlinchedVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class FlinchedVolatileStatusEffectAction : BattleAction
{
    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;

    public FlinchedVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += CancelAttack;
        battleSystem.OnEndOfTurn += EndOfTurnStatusRemoval;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= CancelAttack;
        battleSystem.OnEndOfTurn -= EndOfTurnStatusRemoval;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    public void CancelAttack(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        eventArgs.GetTerraAttack().SetCanceled(true);
        Debug.Log(BattleDialog.FlinchedMsg(terraBattlePosition.GetTerra()));
    }

    public void EndOfTurnStatusRemoval(object sender, BattleEventArgs eventArgs)
    {
        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}