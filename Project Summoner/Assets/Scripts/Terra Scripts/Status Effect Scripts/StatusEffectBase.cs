using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectBase
{
    protected StatusEffectSO statusEffectSO;

    public StatusEffectBase(StatusEffectSO statusEffectSO)
    {
        this.statusEffectSO = statusEffectSO;
    }

    public abstract void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem);

    public abstract void RemoveBattleActions(BattleSystem battleSystem);

    public StatusEffectSO GetStatusEffectSO() { return statusEffectSO; }
}
