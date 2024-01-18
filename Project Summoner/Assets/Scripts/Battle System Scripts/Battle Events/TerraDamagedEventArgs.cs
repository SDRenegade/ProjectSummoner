using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraDamagedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private int? damage;

    public TerraDamagedEventArgs(TerraBattlePosition terraBattlePosition, int? damage, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.damage = damage;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public int? GetDamage() { return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }
}
