using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackLog
{
    private DirectAttackParams directAttackParams;
    private bool isSuccessful;
    private bool isHit;
    private bool isCrit;
    private int? damage;

    public DirectAttackLog(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove terraMove)
    {
        directAttackParams = new DirectAttackParams(attackerPosition, defenderPosition, terraMove);
        isSuccessful = false;
        isHit = false;
        isCrit = false;
        damage = null;
    }

    public DirectAttackLog(DirectAttackParams directAttackParams)
    {
        this.directAttackParams = directAttackParams;
        isSuccessful = false;
        isHit = false;
        isCrit = false;
        damage = null;
    }

    public TerraBattlePosition GetAttackerPosition() { return directAttackParams.GetAttackerPosition(); }

    public TerraBattlePosition GetDefenderPosition() { return directAttackParams.GetDefenderPosition(); }

    public DirectAttackParams GetDirectAttackParams() { return directAttackParams; }

    public void SetDirectAttackParams(DirectAttackParams directAttackParams) { this.directAttackParams = directAttackParams; }

    public bool IsSuccessful() { return isSuccessful; } 

    public void SetSuccessful(bool isSuccessful) { this.isSuccessful = isSuccessful; }

    public bool IsHit() {  return isHit; }

    public void SetHit(bool isHit) { this.isHit = isHit; }

    public bool IsCrit() { return isCrit; }

    public void SetCrit(bool isCrit) {  this.isCrit = isCrit; }

    public int? GetDamage() {  return damage; }

    public void SetDamage(int? damage) { this.damage = damage; }
}
