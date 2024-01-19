using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        battleManager.GetBattleSystem().OpenMenuSelectionUI();
    }
}
