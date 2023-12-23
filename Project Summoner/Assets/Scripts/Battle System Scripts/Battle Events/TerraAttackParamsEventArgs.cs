using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackParamsEventArgs : BattleEventArgs
{
    TerraAttackParams terraAttackParams;

    public TerraAttackParamsEventArgs(TerraAttackParams terraAttackParams, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttackParams = terraAttackParams;
    }

    public TerraAttackParams GetTerraAttackParmas() { return terraAttackParams; }
}
