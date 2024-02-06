using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private StatusEffectSO statusEffectSO;

    public StatusEffectEventArgs(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.statusEffectSO = statusEffectSO;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public StatusEffectSO GetStatusEffectSO() { return statusEffectSO; }
}
