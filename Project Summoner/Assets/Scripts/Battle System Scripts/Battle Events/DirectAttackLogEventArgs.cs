using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackLogEventArgs : BattleEventArgs
{
    private DirectAttackLog directAttackLog;
    private Terra attackingTerra;
    private Terra defendingTerra;

    public DirectAttackLogEventArgs(DirectAttackLog directAttackLog, BattleSystem battleSystem) : base(battleSystem)
    {
        this.directAttackLog = directAttackLog;
        attackingTerra = directAttackLog.GetAttackerPosition().GetTerra();
        defendingTerra = directAttackLog.GetDefenderPosition().GetTerra();
    }

    public DirectAttackLog GetDirectAttackLog() { return directAttackLog; }

    public Terra GetAttackingTerra() {  return attackingTerra; }

    public Terra GetDefendingTerra() { return defendingTerra; }
}
