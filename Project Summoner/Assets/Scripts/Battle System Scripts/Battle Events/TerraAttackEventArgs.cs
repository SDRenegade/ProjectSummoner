using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;

    public TerraAttackEventArgs(TerraAttack terraAttack, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }
}
