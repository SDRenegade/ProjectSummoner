using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectWrapper
{
    [SerializeField] private VolatileStatusEffectBase vStatusEffectBase;
    private BattleAction battleAction;

    public VolatileStatusEffectWrapper(VolatileStatusEffectBase vStatusEffectBase, TerraBattlePosition terraBattlePosition)
    {
        SetVolatileStatusEffectBase(vStatusEffectBase, terraBattlePosition);
    }

    public void RemoveVolatileStatusEffectBattleActions(BattleSystem battleSystem)
    {
        battleAction?.RemoveBattleActions(battleSystem);
    }

    public VolatileStatusEffectBase GetVolatileStatusEffectBase() { return vStatusEffectBase; }

    public void SetVolatileStatusEffectBase(VolatileStatusEffectBase vStatusEffectBase, TerraBattlePosition terraBattlePosition)
    {
        this.vStatusEffectBase = vStatusEffectBase;
        battleAction = (vStatusEffectBase != null) ? vStatusEffectBase.CreateBattleAction(terraBattlePosition) : null;
    }

    public BattleAction GetBattleAction() {  return battleAction; }
}
