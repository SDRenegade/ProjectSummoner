using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackTerraEventArgs : EventArgs
{
    TerraAttack terraAttack;
    private BattleSystem battleSystem;

    public TerraAttackTerraEventArgs(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        this.terraAttack = terraAttack;
        this.battleSystem = battleSystem;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public BattleSystem GetBattleSystem() { return battleSystem; }
}
