using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventArgs : EventArgs
{
    private BattleSystem battleSystem;

    public BattleEventArgs(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public BattleSystem GetBattleSystem() { return battleSystem; }
}
