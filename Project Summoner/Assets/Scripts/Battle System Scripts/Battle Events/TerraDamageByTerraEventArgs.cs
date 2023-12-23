using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraDamageByTerraEventArgs : BattleEventArgs
{
    private TerraAttack terraAttack;
    private TerraAttackLog terraAttackLog;
    private int? damage;

    public TerraDamageByTerraEventArgs(TerraAttack terraAttack, TerraAttackLog terraAttackLog, int? damage, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraAttack = terraAttack;
        this.terraAttackLog = terraAttackLog;
        this.damage = damage;
    }

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public TerraAttackLog GetTerraAttacLog() { return terraAttackLog; }

    public int? GetDamage() { return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }
}
