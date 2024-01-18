using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A set of all causes of damage in battle
public enum BattleDamageCause
{
    SELF,
    TERRA_ATTACK,
    BURN,
    BLIGHT,
    CONFUSION,
    BATTLE_HAZARD
}

public class TerraDamageEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraPosition;
    private BattleDamageCause damageCause;

    public TerraDamageEventArgs(TerraBattlePosition terraPosition, BattleDamageCause damageCause, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraPosition = terraPosition;
        this.damageCause = damageCause;
    }

    public TerraBattlePosition GetTerraPosition() { return terraPosition; }

    public BattleDamageCause GetDamageCause() {  return damageCause; }
}
