using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Stat Resistance")]
public class StatResistanceVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new StatResistanceVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class StatResistanceVolatileStatusEffectAction : BattleAction
{
    private static readonly int TURN_DURATION = 5;

    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private int turnCounter;

    public StatResistanceVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnStatChange += CancelStatChange;
        battleSystem.OnEndOfTurn += EndOfTurnIncrement;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnStatChange -= CancelStatChange;
        battleSystem.OnEndOfTurn -= EndOfTurnIncrement;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    public void CancelStatChange(object sender, StatChangeEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;

        eventArgs.SetCanceled(true);
        Debug.Log(BattleDialog.MistProkedMsg(terraBattlePosition.GetTerra()));
    }

    public void EndOfTurnIncrement(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
        if (turnCounter >= TURN_DURATION)
            RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}