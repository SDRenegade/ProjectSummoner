using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleState
{
    void EnterState(BattleStateManager battleManager);
}
