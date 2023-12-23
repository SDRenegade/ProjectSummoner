using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectWrapper
{
    [SerializeField] private StatusEffectBase statusEffectBase;
    private BattleAction battleAction;

    public StatusEffectWrapper(StatusEffectBase statusEffectBase, Terra terra)
    {
        SetStatusEffectBase(statusEffectBase, terra);
    }

    public void AddStatusEffectBattleActoin(BattleSystem battleSystem, Terra terra)
    {
        if (statusEffectBase == null)
            return;

        battleAction = statusEffectBase.CreateBattleAction(terra);
        battleAction.AddBattleAction(battleSystem);
    }

    public void RemoveStatusEffectBattleAction(BattleSystem battleSystem)
    {
        battleAction?.RemoveBattleAction(battleSystem);
    }

    public StatusEffectBase GetStatusEffectBase() { return statusEffectBase; }

    public void SetStatusEffectBase(StatusEffectBase statusEffectBase, Terra terra)
    {
        this.statusEffectBase = statusEffectBase;
        battleAction = (statusEffectBase != null) ? statusEffectBase.CreateBattleAction(terra) : null;
    }
}
