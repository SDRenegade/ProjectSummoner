using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** End of Turn Event ***
        battleSystem.InvokeOnEndOfTurn();

        battleSystem.UpdateTerraStatusBars();

        Debug.Log("============== Entered End of Turn Battle State ==============");
        battleSystem.GetBattleActionManager().ResetActions(battleSystem);

        battleManager.SwitchState(battleManager.GetStartTurnState());
    }
}
