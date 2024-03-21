using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTurnBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        Debug.Log("============== Entered End of Turn Battle State ==============");
        //*** End of Turn Event ***
        battleSystem.InvokeOnEndOfTurn();

        if(battleSystem.GetBattleActionManager().GetFaintedTerraQueue().Count > 0)
            battleSystem.SwitchFaintedTerra();
        else {
            battleSystem.GetBattleActionManager().ResetActions(battleSystem);
            battleManager.SwitchState(battleManager.GetStartTurnState());
        }
    }
}
