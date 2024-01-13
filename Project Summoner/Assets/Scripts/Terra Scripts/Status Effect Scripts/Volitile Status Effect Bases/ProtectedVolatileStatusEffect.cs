using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Protected")]
public class ProtectedVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new ProtectedVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class ProtectedVolatileStatusEffectAction : BattleAction
{
    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private bool isCooldown;

    public ProtectedVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        isCooldown = false;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += ProtectActivate;
        battleSystem.OnEndOfTurn += EndOfTurnSetCooldown;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= ProtectActivate;
        battleSystem.OnEndOfTurn -= EndOfTurnSetCooldown;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
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
            RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}