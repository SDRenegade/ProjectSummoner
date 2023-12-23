using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAction
{
    void AddBattleAction(BattleSystem battleSystem);

    void RemoveBattleAction(BattleSystem battleSystem);
}
