using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Protected")]
public class ProtectedVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new ProtectedVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class ProtectedVolatileStatusEffect : VolatileStatusEffectBase
{
    private bool isCooldown;

    public ProtectedVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        isCooldown = false;
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += ProtectActivate;
        battleSystem.OnEndOfTurn += EndOfTurnSetCooldown;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= ProtectActivate;
        battleSystem.OnEndOfTurn -= EndOfTurnSetCooldown;
    }

    private void ProtectActivate(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;

        eventArgs.SetCanceled(true);
        Debug.Log(BattleDialog.ProtectActiveMsg(terraBattlePosition.GetTerra()));

        eventArgs.GetBattleSystem().OnDirectAttack -= ProtectActivate;
    }

    private void EndOfTurnSetCooldown(object sender, BattleEventArgs eventArgs)
    {
        if (!isCooldown) {
            isCooldown = true;
        }
        else
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
    }
}