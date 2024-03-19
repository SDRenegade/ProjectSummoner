using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VolatileStatusEffectBase
{
    protected TerraBattlePosition terraBattlePosition;
    protected VolatileStatusEffectSO vStatusEffectSO;

    public VolatileStatusEffectBase(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffectSO = vStatusEffectSO;
    }

    public abstract void AddVolatileStatusEffectListeners(BattleSystem battleSystem);

    public abstract void RemoveVolatileStatusEffectListeners(BattleSystem battleSystem);

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public VolatileStatusEffectSO GetVolatileStatusEffectSO() { return vStatusEffectSO; }
}
