using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOfTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        Debug.Log("============== Entered Start of Turn Battle State ==============");
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** Start of Turn Event ***
        battleSystem.InvokeOnStartOfTurn();

        battleManager.SwitchState(battleManager.GetActionSelectionState());
    }
}
