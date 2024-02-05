using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChargingEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;
    private bool isCanceled;

    public AttackChargingEventArgs(TerraAttack terraAttack, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
        isCanceled = false;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
