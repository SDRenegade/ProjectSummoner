using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedMatchBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        Debug.Log("============ Entered Finished Match State ============");
        battleManager.GetBattleSystem().EndBattle();
    }
}
