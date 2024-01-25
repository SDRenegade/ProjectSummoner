using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Stat Resistance")]
public class StatResistanceVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new StatResistanceVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class StatResistanceVolatileStatusEffect : VolatileStatusEffectBase
{
    private static readonly int TURN_DURATION = 5;

    private int turnCounter;

    public StatResistanceVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        turnCounter = 0;
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnStatChange += CancelStatChange;
        battleSystem.OnEndOfTurn += EndOfTurnIncrement;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnStatChange -= CancelStatChange;
        battleSystem.OnEndOfTurn -= EndOfTurnIncrement;
    }

    public void CancelStatChange(object sender, StatChangeEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.MistProkedMsg(terraBattlePosition.GetTerra()));
        eventArgs.SetCanceled(true);
    }

    public void EndOfTurnIncrement(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
        if (turnCounter >= TURN_DURATION)
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
    }
}