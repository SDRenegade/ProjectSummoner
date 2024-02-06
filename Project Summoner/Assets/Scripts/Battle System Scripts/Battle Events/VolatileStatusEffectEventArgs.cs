using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;

    public VolatileStatusEffectEventArgs(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public VolatileStatusEffectBase GetVolatileStatusEffect() { return vStatusEffect; }
}
