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

    public void AddStatusEffectBattleAction(Terra terra, BattleSystem battleSystem)
    {
        if (statusEffectBase == null)
            return;

        battleAction = statusEffectBase.CreateBattleAction(terra);
        battleAction.AddBattleActions(battleSystem);
    }

    public void RemoveStatusEffectBattleAction(BattleSystem battleSystem)
    {
        battleAction?.RemoveBattleActions(battleSystem);
    }

    public StatusEffectBase GetStatusEffectBase() { return statusEffectBase; }

    public void SetStatusEffectBase(StatusEffectBase statusEffectBase, Terra terra)
    {
        this.statusEffectBase = statusEffectBase;
        battleAction = (statusEffectBase != null) ? statusEffectBase.CreateBattleAction(terra) : null;
    }

    public void SetStatusEffectBase(StatusEffectBase statusEffectBase, Terra terra, BattleSystem battleSystem)
    {
        if (battleAction != null)
            RemoveStatusEffectBattleAction(battleSystem);

        this.statusEffectBase = statusEffectBase;
        battleAction = (statusEffectBase != null) ? statusEffectBase.CreateBattleAction(terra) : null;

        AddStatusEffectBattleAction(terra, battleSystem);
    }
}
