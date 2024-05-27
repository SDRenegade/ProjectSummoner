using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** Init Battle State Event ***
        battleSystem.InvokeOnStartOfTurn();

        battleSystem.GetBattleHUD().InitBattleHUD(battleSystem);
        battleSystem.GetBattleStage().InitBattleStage(battleSystem.GetBattlefield().GetTerraBattlePositionList());
        InitBattleActions(battleSystem);

        battleSystem.UpdateTerraStatusBars();

        Debug.Log("============== Entered Init Battle State ==============");
        // Removed temporarily for testing
        //BattleSequenceManager.GetInstance().StartIntroSequence(battleSystem.GetBattlefield());
        battleManager.SwitchState(battleManager.GetStartTurnState());
    }

    private void InitBattleActions(BattleSystem battleSystem)
    {
        Battlefield battlefield = battleSystem.GetBattlefield();

        //Initialize the existing status conditions and items on the terra in the event system
        for (int i = 0; i < battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            TerraBattlePosition terraBattlePosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[i];
            if (terraBattlePosition.GetTerra() == null)
                continue;

            terraBattlePosition.GetTerra().GetStatusEffect()?.AddStatusListeners(terraBattlePosition, battleSystem); ;
            //--- (Temp) Hard-coding the leading terra held item until new system is added ---
            terraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Persim Berry").CreateItemBase());
            terraBattlePosition.GetTerra().GetHeldItem()?.AddItemListeners(terraBattlePosition, battleSystem);

            //Logging the item that each leading terra is holding
            if (terraBattlePosition.GetTerra().GetHeldItem() != null)
                Debug.Log(terraBattlePosition.GetTerra() + " is holding the item: " + terraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());
        }

        for (int i = 0; i < battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            TerraBattlePosition terraBattlePosition = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[i];
            if (terraBattlePosition.GetTerra() == null)
                continue;

            terraBattlePosition.GetTerra().GetStatusEffect()?.AddStatusListeners(terraBattlePosition, battleSystem);
            //--- (Temp) Hard-coding the leading terra held item until new system is added ---
            //terraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Leftovers").CreateItemBase());
            terraBattlePosition.GetTerra().GetHeldItem()?.AddItemListeners(terraBattlePosition, battleSystem);

            //Logging the item that each leading terra is holding
            if (terraBattlePosition.GetTerra().GetHeldItem() != null)
                Debug.Log(terraBattlePosition.GetTerra() + " is holding the item: " + terraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());
        }
    }
}
