using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackLog
{
    private TerraBattlePosition attackerPosition;
    private TerraBattlePosition defenderPosition;
    private bool isSuccessfulHit;
    private bool isCrit;
    private int? damage;

    public TerraAttackLog(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition)
    {
        this.attackerPosition = attackerPosition;
        this.defenderPosition = defenderPosition;
        isSuccessfulHit = false;
        isCrit = false;
        damage = null;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public TerraBattlePosition GetDefenderPosition() { return defenderPosition; }

    public bool IsSuccessfulHit() {  return isSuccessfulHit; }

    public void SetSuccessfulHit(bool successfulHit) { this.isSuccessfulHit = successfulHit; }

    public bool IsCrit() { return isCrit; }

    public void SetCrit(bool isCrit) {  this.isCrit = isCrit; }

    public int? GetDamage() {  return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }

    public void AddDamage(int? damage)
    {
        if (damage == null)
            return;

        this.damage = (this.damage == null) ? damage : this.damage + damage;
    }
}
