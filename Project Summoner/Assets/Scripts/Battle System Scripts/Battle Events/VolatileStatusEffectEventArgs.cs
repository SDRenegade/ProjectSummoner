using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra terra;
    private VolatileStatusEffectBase vStatusEffect;
    private bool isCanceled;

    public VolatileStatusEffectEventArgs(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        terra = terraBattlePosition.GetTerra();
        this.vStatusEffect = vStatusEffect;
        isCanceled = false;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GetTerra() {  return terra; }

    public VolatileStatusEffectBase GetVolatileStatusEffect() { return vStatusEffect; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
