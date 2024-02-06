using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectAddedEventArgs : StatusEffectEventArgs
{
    private bool isCanceled;

    public StatusEffectAddedEventArgs(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO, BattleSystem battleSystem) : base(terraBattlePosition, statusEffectSO, battleSystem)
    {
        isCanceled = false;
    }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
