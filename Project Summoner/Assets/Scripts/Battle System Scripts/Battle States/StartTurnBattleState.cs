using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnBattleState : BattleState
{

    public void EnterState(BattleStateManager battleManager)
    {
        //*** Entering Start Turn State Event ***
        battleManager.GetBattleSystem().InvokeOnStartOfTurn();

        Debug.Log("============== Entered Start of Turn Battle State ==============");
        //TODO Set main camera to start moving along track
        battleManager.SwitchState(battleManager.GetActionSelectionState());
    }
}
