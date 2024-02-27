using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TerraMoveSO : ScriptableObject
{
    [SerializeField] protected string moveName;
    [SerializeField, TextArea] protected string description;
    [SerializeField] protected TerraType terraType;
    [SerializeField] protected DamageType damageType;
    [SerializeField] protected MovePriority baseMovePriority;
    [SerializeField] protected IntNullable baseDamage;
    [SerializeField] protected IntNullable baseAccuracy;
    [SerializeField] protected int basePP;
    [SerializeField] protected bool isTargetSelectable;
    [SerializeField] protected bool hasChargeTurn;
    [SerializeField] protected bool hasRechargeTurn;

    public abstract TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack);
    
    public string GetMoveName() { return moveName; }
    
    public string GetDescription() { return description; }
    
    public TerraType GetTerraType() {  return terraType; }
    
    public DamageType GetDamageType() {  return damageType; }
    
    public MovePriority GetBaseMovePriority() {  return baseMovePriority; }
    
    public IntNullable GetBaseDamage() { return baseDamage; }
    
    public IntNullable GetBaseAccuracy() { return baseAccuracy; }
    
    public int GetBasePP() { return basePP; }

    public bool IsTargetSelectable() { return isTargetSelectable;}

    public bool HasChargeTurn() {  return hasChargeTurn; }

    public bool HasRechargeTurn() {  return hasRechargeTurn; }

    public override string ToString() { return moveName; }
}
