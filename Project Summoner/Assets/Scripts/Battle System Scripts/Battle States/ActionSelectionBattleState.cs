using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        TerraBattlePosition[] primarySidePositions = battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
        for (int i = 0; i < primarySidePositions.Length; i++)
            ProcessActionSelection(battleSystem.GetPrimarySideAI(), primarySidePositions[i], battleSystem);

        TerraBattlePosition[] secondarySidePositions = battleSystem.GetBattlefield().GetSecondaryBattleSide().GetTerraBattlePositionArr();
        for (int i = 0; i < secondarySidePositions.Length; i++)
            ProcessActionSelection(battleSystem.GetSecondarySideAI(), secondarySidePositions[i], battleSystem);
    }

    private void ProcessActionSelection(BattleAI battleAI, TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        //*** Entering Action Selection Event ***
        EnteringActionSelectionEventArgs enteringActionSelectionEventArgs = battleSystem.InvokeOnEnteringActionSelection(terraBattlePosition);

        if (enteringActionSelectionEventArgs.IsSkipActionSelection())
            battleSystem.GetBattleActionManager().AddReadyBattlePosition();
        else if (battleAI == null)
            battleSystem.GetBattleHUD().OpenMenuSelectionUI();
        else {
            battleAI.PerformAction(terraBattlePosition, enteringActionSelectionEventArgs.GetDisabledMoveIndicies(), battleSystem);
            battleSystem.GetBattleActionManager().AddReadyBattlePosition();
        }

        //Check if all battle positions are ready. If so, switch to combat state.
        if (battleSystem.GetBattleActionManager().IsAllBattlePositionsReady())
            battleSystem.EndActionSelection();
    }
}
