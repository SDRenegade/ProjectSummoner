using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackParams
{
    private TerraBattlePosition attackerPosition;
    private TerraBattlePosition defenderPosition;
    private TerraMove move;
    private List<TerraType> attackerTerraTypeList;
    private List<TerraType> defenderTerraTypeList;
    private TerraType moveTerraType;
    private int hitCount;
    private float accuracyModifier;
    private bool isMustHit;
    private float evasivenessModifier;
    private float critModifier;
    private bool isCritable;
    private float damageModifier;
    private bool isDamageStepCanceled;

    public DirectAttackParams(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defenderPosition = defenderPosition;
        this.move = move;
        attackerTerraTypeList = new List<TerraType>();
        foreach(TerraType type in attackerPosition.GetTerra().GetTerraBase().GetTerraTypes())
            attackerTerraTypeList.Add(type);
        defenderTerraTypeList = new List<TerraType>();
        foreach (TerraType type in defenderPosition.GetTerra().GetTerraBase().GetTerraTypes())
            defenderTerraTypeList.Add(type);
        moveTerraType = move.GetMoveSO().GetTerraType();
        hitCount = 1;
        accuracyModifier = 1f;
        isMustHit = false;
        evasivenessModifier = 1f;
        critModifier = 1f;
        isCritable = true;
        damageModifier = 1f;
        isDamageStepCanceled = false;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public TerraBattlePosition GetDefenderPosition() { return defenderPosition; }

    public TerraMove GetMove() { return move; }

    public List<TerraType> GetAttackerTerraTypeList() { return attackerTerraTypeList; }

    public void SetAttackerTerraTypeList(List<TerraType> attackerTerraTypeList) { this.attackerTerraTypeList = attackerTerraTypeList; }

    public List<TerraType> GetDefenderTerraTypeList() { return defenderTerraTypeList; }

    public void SetDefenderTerraTypeList(List<TerraType> defenderTerraTypeList) { this.defenderTerraTypeList = defenderTerraTypeList; }

    public TerraType GetMoveTerraType() { return moveTerraType; }

    public void SetMoveTerraType(TerraType moveTerraType) { this.moveTerraType = moveTerraType; }

    public int GetHitCount() { return hitCount; }

    public void SetHitCount(int hitCount) { this.hitCount = hitCount; }

    public float GetAccuracyModifier() { return accuracyModifier; }

    public void SetAccuracyModifier(float accuracyModifier) { this.accuracyModifier = accuracyModifier; }

    public void AddAccuracyModifier(float accuracyModifier) { this.accuracyModifier *= accuracyModifier; }

    public bool IsMustHit() { return isMustHit; }

    public void SetMustHit(bool isMustHit) { this.isMustHit = isMustHit; }

    public float GetEvasivenessModifier() { return evasivenessModifier; }

    public void SetEvasivenessModifier(float evasivenessModifier) { this.evasivenessModifier = evasivenessModifier; }

    public void AddEvasivenessModifier(float evasivenessModifier) { this.evasivenessModifier *= evasivenessModifier; }

    public float GetCritModifier() { return critModifier; }

    public void SetCritModifier(float critModifier) { this.critModifier = critModifier; }

    public void AddCritModifier(float critModifier) { this.critModifier *= critModifier; }

    public bool IsCritable() { return isCritable; }

    public void SetCritable(bool isCritable) { this.isCritable = isCritable; }

    public float GetDamageModifier() { return damageModifier; }

    public void SetDamageModifier(float damageModifier) { this.damageModifier = damageModifier; }

    public void AddDamageModifier(float damageModifier) { this.damageModifier *= damageModifier; }

    public bool IsDamageStepCanceled() {  return isDamageStepCanceled; }

    public void SetDamageStepCanceled(bool isDamageStepCanceled) { this.isDamageStepCanceled = isDamageStepCanceled; }
}
