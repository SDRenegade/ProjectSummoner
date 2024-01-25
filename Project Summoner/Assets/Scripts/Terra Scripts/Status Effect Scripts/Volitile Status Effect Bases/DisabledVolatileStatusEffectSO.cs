using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Disabled")]
public class DisabledVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new DisabledVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class DisabledVolatileStatusEffect : VolatileStatusEffectBase
{
    private static readonly int TURN_DURATION = 5;

    private int? disabledMoveIndex;
    private int turnCounter;

    public DisabledVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        disabledMoveIndex = null;
        turnCounter = 0;
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        FindMoveIndex(battleSystem.GetBattleActionManager().GetCurrentTurnLog());
        if (disabledMoveIndex == null)
            FindMoveIndex(battleSystem.GetBattleActionManager().GetPreviousTurnLog());

        if (disabledMoveIndex != null) {
            battleSystem.OnEnteringActionSelection += EnteringActionSelectionAction;
            battleSystem.OnOpeningMoveSelectionUI += OpeningMoveSelectionUIAction;
            battleSystem.OnEndOfTurn += EndOfTurnIncrement;
        }
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringActionSelection -= EnteringActionSelectionAction;
        battleSystem.OnOpeningMoveSelectionUI -= OpeningMoveSelectionUIAction;
        battleSystem.OnEndOfTurn -= EndOfTurnIncrement;
    }

    private void EnteringActionSelectionAction(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition().GetTerra() != terraBattlePosition.GetTerra())
            return;

        if (disabledMoveIndex != null)
            eventArgs.AddDisabledMoveIndex((int)disabledMoveIndex);
    }

    private void OpeningMoveSelectionUIAction(object sender, OpeningMoveSelectionUIEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition().GetTerra() != terraBattlePosition.GetTerra())
            return;

        if (disabledMoveIndex != null)
            eventArgs.AddDisabledMoveIndex((int)disabledMoveIndex);
    }

    private void EndOfTurnIncrement(object sneder, BattleEventArgs eventArgs)
    {
        turnCounter++;
        if (turnCounter >= TURN_DURATION)
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
    }

    private bool FindMoveIndex(List<TerraAttack> terraAttackList)
    {
        if (terraAttackList == null)
            return false;

        foreach (TerraAttack terraAttack in terraAttackList) {
            if (terraAttack.GetAttackerPosition() == terraBattlePosition) {
                for (int i = 0; i < terraBattlePosition.GetTerra().GetMoves().Count; i++) {
                    if (terraBattlePosition.GetTerra().GetMoves()[i] == terraAttack.GetMove()) {
                        disabledMoveIndex = i;
                        return true;
                    }
                }
            }
        }

        return false;
    }
}