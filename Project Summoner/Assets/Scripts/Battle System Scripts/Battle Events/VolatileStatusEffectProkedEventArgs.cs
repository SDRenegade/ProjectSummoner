using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectProkedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra terra;
    private VolatileStatusEffectBase vStatusEffectBase;
    private bool isProked;
    private bool isEffectRemoved;

    public VolatileStatusEffectProkedEventArgs(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffectBase, bool isProked, bool isEffectRemoved, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        terra = terraBattlePosition.GetTerra();
        this.vStatusEffectBase = vStatusEffectBase;
        this.isProked = isProked;
        this.isEffectRemoved = isEffectRemoved;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GetTerra() { return terra; }

    public VolatileStatusEffectBase GetVolatileStatusEffectBase() { return vStatusEffectBase; }

    public bool IsProked() { return isProked; }

    public bool IsEffectRemoved() { return isEffectRemoved; }
}
