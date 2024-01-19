using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackEventArgs : BattleEventArgs
{
    private DirectAttackParams directAttackParams;
    private bool isCanceled;

    public DirectAttackEventArgs(DirectAttackParams directAttackParams, BattleSystem battleSystem) : base(battleSystem)
    {
        this.directAttackParams = directAttackParams;
        isCanceled = false;
    }

    public DirectAttackParams GetDirectAttackParams() { return directAttackParams; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) {  this.isCanceled = isCanceled; }
}
