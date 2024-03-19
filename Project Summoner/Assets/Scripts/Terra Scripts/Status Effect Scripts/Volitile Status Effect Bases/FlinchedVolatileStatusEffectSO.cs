using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Flinched")]
public class FlinchedVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new FlinchedVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class FlinchedVolatileStatusEffect : VolatileStatusEffectBase
{
    public FlinchedVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO) {}

    public override void AddVolatileStatusEffectListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += CancelAttack;
        battleSystem.OnEndOfTurn += EndOfTurnStatusRemoval;
    }

    public override void RemoveVolatileStatusEffectListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= CancelAttack;
        battleSystem.OnEndOfTurn -= EndOfTurnStatusRemoval;
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
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
    }
}