using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Special Barrier")]
public class SpecialBarrierVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new SpecialBarrierVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class SpecialBarrierVolatileStatusEffect : VolatileStatusEffectBase
{
    private static readonly int DEFAULT_TURN_DURATION = 5;

    private int turnDuration;
    private int turnCounter;

    public SpecialBarrierVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        turnDuration = DEFAULT_TURN_DURATION;
        turnCounter = 0;
    }

    public override void AddVolatileStatusEffectListeners(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += HalfIncommingSpecialDamage;
        battleSystem.OnEndOfTurn += IncrementTurnCounter;
    }

    public override void RemoveVolatileStatusEffectListeners(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= HalfIncommingSpecialDamage;
        battleSystem.OnEndOfTurn -= IncrementTurnCounter;
    }

    //Used for items that effect the turn duration of the barrier effect, such as the Light Clay
    public void SetTurnDuration(int turnDuration)
    {
        if (turnDuration <= 0)
            return;

        this.turnDuration = turnDuration;
    }

    private void HalfIncommingSpecialDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetTerraAttack().GetMove().GetMoveSO().GetDamageType() != DamageType.SPECIAL)
            return;

        eventArgs.GetDirectAttackLog().SetDamage(eventArgs.GetDirectAttackLog().GetDamage() / 2);
    }

    private void IncrementTurnCounter(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
        Debug.Log("Turn Duration: " + turnDuration + " TurnCounter: " + turnCounter);
        if (turnCounter >= turnDuration) {
            Debug.Log(BattleDialog.LightScreenExpiredMsg(terraBattlePosition.GetTerra()));
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
        }
    }
}