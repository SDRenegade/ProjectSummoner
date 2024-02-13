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

        //Check if all battle positions are ready. If so, switch to combat state. Else, open action menu
        //for next terra in action slection queue.
        if (battleSystem.GetBattleActionManager().IsAllBattlePositionsReady())
            battleSystem.EndActionSelection();
        else
            battleSystem.GetBattleHUD().OpenMenuSelectionUI();
    }

    private void ProcessActionSelection(BattleAI battleAI, TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        //*** Entering Action Selection Event ***
        EnteringActionSelectionEventArgs enteringActionSelectionEventArgs = battleSystem.InvokeOnEnteringActionSelection(terraBattlePosition);

        if (enteringActionSelectionEventArgs.IsSkipActionSelection()) {
            battleSystem.GetBattleActionManager().AddReadyBattlePosition();
            return;
        }

        //Checks if there is already a terra attack queued from last turn for this terra battle position
        for (int i = 0; i < battleSystem.GetBattleActionManager().GetTerraAttackList().Count; i++) {
            if (battleSystem.GetBattleActionManager().GetTerraAttackList()[i].GetAttackerPosition() == terraBattlePosition) {
                battleSystem.GetBattleActionManager().AddReadyBattlePosition();
                return;
            }
        }

        //If there is no battle AI (the terra belongs to a player) add the terraBattlePosition to the
        //action slection queue. Else, call the battle AI perform action method.
        if (battleAI == null)
            battleSystem.GetBattleActionManager().AddTerraToActionSelectionQueue(terraBattlePosition);
        else {
            battleAI.PerformAction(terraBattlePosition, enteringActionSelectionEventArgs.GetDisabledMoveIndicies(), battleSystem);
            battleSystem.GetBattleActionManager().AddReadyBattlePosition();
        }
    }
}
