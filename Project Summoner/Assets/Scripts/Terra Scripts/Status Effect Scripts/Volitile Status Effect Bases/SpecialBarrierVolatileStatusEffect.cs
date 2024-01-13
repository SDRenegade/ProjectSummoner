using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Special Barrier")]
public class SpecialBarrierVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new SpecialBarrierVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class SpecialBarrierVolatileStatusEffectAction : BattleAction
{
    private static readonly int TURN_DURATION = 5;

    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private int turnCounter;

    public SpecialBarrierVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += HalfIncommingSpecialDamage;
        battleSystem.OnEndOfTurn += IncrementTurnCounter;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= HalfIncommingSpecialDamage;
        battleSystem.OnEndOfTurn -= IncrementTurnCounter;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    private void HalfIncommingSpecialDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetTerraAttack().GetMove().GetMoveBase().GetDamageType() != DamageType.SPECIAL)
            return;

        eventArgs.SetDamage(eventArgs.GetDamage() / 2);
    }

    private void IncrementTurnCounter(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
        if(turnCounter >= TURN_DURATION) {
            RemoveBattleActions(eventArgs.GetBattleSystem());
            Debug.Log(BattleDialog.LightScreenExpiredMsg(terraBattlePosition.GetTerra()));
        }
    }
}