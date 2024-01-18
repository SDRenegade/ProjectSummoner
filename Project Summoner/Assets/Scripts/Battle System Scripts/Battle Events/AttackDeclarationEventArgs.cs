using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDeclarationEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;

    public AttackDeclarationEventArgs(TerraAttack terraAttack, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }
}
