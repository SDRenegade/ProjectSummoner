using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        //*** Entering Action Selection State Event ***
        battleManager.GetBattleSystem().InvokeOnEnteringActionSelectionState();

        battleManager.GetBattleSystem().GetBattleHUD().OpenMenuSelectionUI();
    }
}
