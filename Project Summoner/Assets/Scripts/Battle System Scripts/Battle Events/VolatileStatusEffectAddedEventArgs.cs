using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectAddedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private bool isCanceled;

    public VolatileStatusEffectAddedEventArgs(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        isCanceled = false;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public VolatileStatusEffectBase GetVolatileStatusEffect() { return vStatusEffect; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
