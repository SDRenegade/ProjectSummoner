using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraDamageByTerraEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;
    private DirectAttackLog directAttackLog;
    private int? damage;

    public TerraDamageByTerraEventArgs(TerraAttack terraAttack, DirectAttackLog directAttackLog, int? damage, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
        this.directAttackLog = directAttackLog;
        this.damage = damage;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public DirectAttackLog GetDirectAttackLog() { return directAttackLog; }

    public int? GetDamage() { return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }
}
