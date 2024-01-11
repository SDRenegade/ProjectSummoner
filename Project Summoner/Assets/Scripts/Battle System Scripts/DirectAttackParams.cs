using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackParams
{
    private TerraBattlePosition attackerPosition;
    private TerraBattlePosition defenderPosition;
    private TerraMove move;
    private int hitCount;
    private float accuracyModifier;
    private bool isMustHit;
    private bool isDamageStepCanceled;
    private float critModifier;
    private bool isCritable;
    private float damageModifier;

    public DirectAttackParams(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defenderPosition = defenderPosition;
        this.move = move;
        hitCount = 1;
        accuracyModifier = 1f;
        isMustHit = false;
        isDamageStepCanceled = false;
        critModifier = 1f;
        isCritable = true;
        damageModifier = 1f;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public TerraBattlePosition GetDefenderPosition() { return defenderPosition; }

    public TerraMove GetMove() { return move; }
    
    public int GetHitCount() { return hitCount; }

    public void SetHitCount(int hitCount) { this.hitCount = hitCount; }

    public float GetAccuracyModifier() { return accuracyModifier; }

    public void SetAccuracyModifier(float accuracyModifier) { this.accuracyModifier = accuracyModifier; }

    public bool IsMustHit() { return isMustHit; }

    public void SetMustHit(bool isMustHit) { this.isMustHit = isMustHit; }

    public bool IsDamageStepCanceled() {  return isDamageStepCanceled; }

    public void SetDamageStepCanceled(bool isDamageStepCanceled) { this.isDamageStepCanceled = isDamageStepCanceled; }

    public float GetCritModifier() { return critModifier; }

    public void SetCritModifier(float critModifier) { this.critModifier = critModifier; }

    public bool IsCritable() { return isCritable; }

    public void SetCritable(bool isCritable) { this.isCritable = isCritable; }

    public float GetDamageModifier() { return damageModifier; }

    public void SetDamageModifier(float damageModifier) { this.damageModifier = damageModifier; }
}
