using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Disabled")]
public class DisabledVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new DisabledVolatileStatusEffectAction(terraBattlePosition);
    }
}

public class DisabledVolatileStatusEffectAction : BattleAction
{
    private static readonly int TURN_DURATION = 5;

    private TerraBattlePosition terraBattlePosition;
    private int? disabledMoveIndex;
    private int turnCounter;

    public DisabledVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
        disabledMoveIndex = null;
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
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

    public void RemoveBattleActions(BattleSystem battleSystem)
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
            RemoveBattleActions(eventArgs.GetBattleSystem());
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