using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraDamageByTerraEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;
    private DirectAttackLog directAttackLog;

    public TerraDamageByTerraEventArgs(TerraAttack terraAttack, DirectAttackLog directAttackLog, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
        this.directAttackLog = directAttackLog;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public DirectAttackLog GetDirectAttackLog() { return directAttackLog; }
}
