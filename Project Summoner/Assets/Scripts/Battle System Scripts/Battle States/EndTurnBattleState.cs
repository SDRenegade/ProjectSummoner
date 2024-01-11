using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** End of Turn Event ***
        battleSystem.InvokeOnEndOfTurn();

        Debug.Log("============== Entered End of Turn Battle State ==============");
        battleSystem.GetBattleActionManager().ResetActions();

        battleManager.SwitchState(battleManager.GetStartTurnState());
    }
}
