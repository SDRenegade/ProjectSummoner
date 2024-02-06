using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectAddedEventArgs : VolatileStatusEffectEventArgs
{
    private bool isCanceled;

    public VolatileStatusEffectAddedEventArgs(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect, BattleSystem battleSystem) : base(terraBattlePosition, vStatusEffect, battleSystem)
    {
        isCanceled = false;
    }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
