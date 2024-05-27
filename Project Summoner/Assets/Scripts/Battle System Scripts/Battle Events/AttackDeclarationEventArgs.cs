using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDeclarationEventArgs : BattleSequenceEventArgs
{
    private TerraAttack terraAttack;

    public AttackDeclarationEventArgs(TerraAttack terraAttack, BattleSystem battleSystem) : base(terraAttack, battleSystem)
    {
        this.terraAttack = terraAttack;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }
}
