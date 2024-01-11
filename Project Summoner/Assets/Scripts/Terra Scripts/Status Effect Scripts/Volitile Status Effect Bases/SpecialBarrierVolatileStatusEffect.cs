using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Special Barrier")]
public class SpecialBarrierVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new SpecialBarrierVolatileStatusEffectAction(terraBattlePosition);
    }
}

public class SpecialBarrierVolatileStatusEffectAction : BattleAction
{
    private static readonly int TURN_DURATION = 5;

    private TerraBattlePosition terraBattlePosition;
    private int turnCounter;

    public SpecialBarrierVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
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
            terraBattlePosition.RemoveVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Special Barrier"));
            Debug.Log(BattleDialog.LightScreenExpiredMsg(terraBattlePosition.GetTerra()));
        }
    }
}