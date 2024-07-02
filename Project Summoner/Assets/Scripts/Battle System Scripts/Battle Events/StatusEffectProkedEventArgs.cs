using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectProkedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra terra;
    private StatusEffectSO statusEffectSO;
    private bool isProked;
    private bool isEffectRemoved;

    public StatusEffectProkedEventArgs(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO, bool isProked, bool isEfectRemoved, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        terra = terraBattlePosition.GetTerra();
        this.statusEffectSO = statusEffectSO;
        this.isProked = isProked;
        this.isEffectRemoved = isEfectRemoved;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GetTerra() { return terra; }

    public StatusEffectSO GetStatusEffectSO() { return statusEffectSO; }

    public bool IsProked() { return isProked; }

    public bool IsEffectRemoved() {  return isEffectRemoved; }
}
