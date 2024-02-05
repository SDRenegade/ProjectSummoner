using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStatusEffectRollEventArgs : BattleEventArgs
{
    private TerraBattlePosition attackerPosition;
    private TerraBattlePosition defenderPosition;
    private VolatileStatusEffectSO vStatusEffectSO;
    private float rollOdds;

    public VolatileStatusEffectRollEventArgs(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, VolatileStatusEffectSO vStatusEffectSO, float rollOdds, BattleSystem battleSystem) : base(battleSystem)
    {
        this.attackerPosition = attackerPosition;
        this.defenderPosition = defenderPosition;
        this.vStatusEffectSO = vStatusEffectSO;
        this.rollOdds = rollOdds;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public TerraBattlePosition GetDefenderPosition() { return defenderPosition; }

    public VolatileStatusEffectSO GetVolatileStatusEffect() { return vStatusEffectSO; }

    public float GetRollOdds() {  return rollOdds; }

    public void SetRollOdds(float rollOdds) { this.rollOdds = rollOdds; }

    public void AddRollOddsModifier(float rollOddsModifier) { rollOdds *= rollOddsModifier; }
}
