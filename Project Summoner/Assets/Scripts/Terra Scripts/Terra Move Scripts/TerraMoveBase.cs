using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TerraMoveBase : ScriptableObject
{
    [SerializeField] protected string moveName;
    [SerializeField] [TextArea] protected string description;
    [SerializeField] protected TerraType moveType;
    [SerializeField] protected DamageType damageType;
    [SerializeField] protected MovePriority baseMovePriority;
    [SerializeField] protected IntNullable baseDamage;
    [SerializeField] protected IntNullable baseAccuracy;
    [SerializeField] protected int basePP;

    public abstract TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack);
    
    public string GetMoveName() { return moveName; }
    
    public string GetDescription() { return description; }
    
    public TerraType GetMoveType() {  return moveType; }
    
    public DamageType GetDamageType() {  return damageType; }
    
    public MovePriority GetBaseMovePriority() {  return baseMovePriority; }
    
    public IntNullable GetBaseDamage() { return baseDamage; }
    
    public IntNullable GetBaseAccuracy() { return baseAccuracy; }
    
    public int GetBasePP() { return basePP; }
}
