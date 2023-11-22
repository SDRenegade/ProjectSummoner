using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        //Check Post combat berries, items, status effects, and abilities
        Debug.Log("Entered End of Turn Battle State");
        battleManager.GetBattleSystem().InvokeOnEndTurn();
        battleManager.SwitchState(battleManager.GetStartTurnState());
    }
}
