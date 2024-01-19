using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnBattleState : BattleState
{

    public void EnterState(BattleStateManager battleManager)
    {
        //Set main camera to start moving along track
        //Check if there are any item actives
        //Check for environment effects
        Debug.Log("Entered Start of Turn Battle State");
        battleManager.SwitchState(battleManager.GetActionSelectionState());
    }
}
