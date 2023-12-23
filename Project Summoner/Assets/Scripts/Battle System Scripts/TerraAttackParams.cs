using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackParams
{
    private TerraBattlePosition attackerPosition;
    private TerraBattlePosition defenderPosition;
    private TerraMove move;
    private int hitCount;
    private float accuracyModifier;
    private bool isMustHit;
    private float critModifier;
    private bool isCanCrit;
    private float damageModifier;

    public TerraAttackParams(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defenderPosition = defenderPosition;
        this.move = move;
        hitCount = 1;
        accuracyModifier = 1f;
        isMustHit = false;
        critModifier = 1f;
        isCanCrit = true;
        damageModifier = 1f;
    }

    //TODO Might want to move this out of this class
    public int CalculateAttackAccuracy()
    {
        return (int)(move.GetMoveBase().GetBaseAccuracy().GetValue() / StatStagesExtension.GetStatStageMultiplier(defenderPosition.GetEvasivenessStage()) * accuracyModifier);
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

    public float GetCritModifier() { return critModifier; }

    public void SetCritModifier(float critModifier) { this.critModifier = critModifier; }

    public bool IsCanCrit() { return isCanCrit; }

    public void SetCanCrit(bool isCanCrit) { this.isCanCrit = isCanCrit; }

    public float GetDamageModifier() { return damageModifier; }

    public void SetDamageModifier(float damageModifier) { this.damageModifier = damageModifier; }
}
