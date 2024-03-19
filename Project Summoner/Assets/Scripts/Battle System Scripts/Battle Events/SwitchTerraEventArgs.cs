using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTerraEventArgs : BattleEventArgs
{
    private TerraSwitch terraSwitch;
    private bool isCanceled;

    public SwitchTerraEventArgs(TerraSwitch terraSwitch, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraSwitch = terraSwitch;
        isCanceled = false;
    }

    public TerraSwitch GetTerraSwitch() { return terraSwitch; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
