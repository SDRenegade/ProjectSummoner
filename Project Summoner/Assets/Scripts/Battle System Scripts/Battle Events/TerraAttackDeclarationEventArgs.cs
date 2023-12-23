using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackDeclarationEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;

    public TerraAttackDeclarationEventArgs(TerraAttack terraAttack, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }
}
