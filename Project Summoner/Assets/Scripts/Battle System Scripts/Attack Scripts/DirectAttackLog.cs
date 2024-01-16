using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackLog
{
    private DirectAttackParams directAttackParams;
    private bool isSuccessfulHit;
    private bool isCrit;
    private int? damage;

    public DirectAttackLog(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove terraMove)
    {
        directAttackParams = new DirectAttackParams(attackerPosition, defenderPosition, terraMove);
        isSuccessfulHit = false;
        isCrit = false;
        damage = null;
    }

    public DirectAttackLog(DirectAttackParams directAttackParams)
    {
        this.directAttackParams = directAttackParams;
        isSuccessfulHit = false;
        isCrit = false;
        damage = null;
    }

    public TerraBattlePosition GetAttackerPosition() { return directAttackParams.GetAttackerPosition(); }

    public TerraBattlePosition GetDefenderPosition() { return directAttackParams.GetDefenderPosition(); }

    public DirectAttackParams GetDirectAttackParams() { return directAttackParams; }

    public void SetDirectAttackParams(DirectAttackParams directAttackParams) { this.directAttackParams = directAttackParams; }

    public bool IsSuccessfulHit() {  return isSuccessfulHit; }

    public void SetSuccessfulHit(bool successfulHit) { this.isSuccessfulHit = successfulHit; }

    public bool IsCrit() { return isCrit; }

    public void SetCrit(bool isCrit) {  this.isCrit = isCrit; }

    public int? GetDamage() {  return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }
}
