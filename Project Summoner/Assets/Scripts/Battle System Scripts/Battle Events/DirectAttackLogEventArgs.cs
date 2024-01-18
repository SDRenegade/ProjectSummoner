using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackLogEventArgs : BattleEventArgs
{
    private DirectAttackLog directAttackLog;

    public DirectAttackLogEventArgs(DirectAttackLog directAttackLog, BattleSystem battleSystem) : base(battleSystem)
    {
        this.directAttackLog = directAttackLog;
    }

    public DirectAttackLog GetDirectAttackLog() { return directAttackLog; }
}
