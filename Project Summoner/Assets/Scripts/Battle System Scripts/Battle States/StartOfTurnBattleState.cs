using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOfTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** Start of Turn Event ***
        battleSystem.InvokeOnStartOfTurn();

        battleSystem.UpdateTerraStatusBars();

        Debug.Log("============== Entered Start of Turn Battle State ==============");
        battleManager.SwitchState(battleManager.GetActionSelectionState());
    }
}
