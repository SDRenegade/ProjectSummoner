using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        //*** Entering End Turn State Event ***
        battleManager.GetBattleSystem().InvokeOnEnteringEndTurnState();

        Debug.Log("============== Entered End of Turn Battle State ==============");
        battleManager.SwitchState(battleManager.GetStartTurnState());
    }
}
