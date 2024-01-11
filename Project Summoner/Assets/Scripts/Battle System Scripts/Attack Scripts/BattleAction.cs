using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAction
{
    void AddBattleActions(BattleSystem battleSystem);

    void RemoveBattleActions(BattleSystem battleSystem);
}
