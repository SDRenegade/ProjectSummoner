using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectWrapper
{
    [SerializeField] private StatusEffectBase statusEffectBase;
    private BattleAction battleAction;

    public StatusEffectWrapper(StatusEffectBase statusEffectBase)
    {
        this.statusEffectBase = statusEffectBase;
    }

    public void AddStatusEffectBattleAction(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        if (statusEffectBase == null)
            return;

        battleAction = statusEffectBase.CreateBattleAction(terraBattlePosition);
        battleAction.AddBattleActions(battleSystem);
    }

    public void RemoveStatusEffectBattleAction(BattleSystem battleSystem)
    {
        battleAction?.RemoveBattleActions(battleSystem);
    }

    public StatusEffectBase GetStatusEffectBase() { return statusEffectBase; }

    public void SetStatusEffectBase(StatusEffectBase statusEffectBase) { this.statusEffectBase = statusEffectBase; }

    public void SetStatusEffectBase(StatusEffectBase statusEffectBase, TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        if (battleAction != null)
            RemoveStatusEffectBattleAction(battleSystem);

        this.statusEffectBase = statusEffectBase;
        battleAction = (statusEffectBase != null) ? statusEffectBase.CreateBattleAction(terraBattlePosition) : null;

        AddStatusEffectBattleAction(terraBattlePosition, battleSystem);
    }
}
