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
    [SerializeField] protected MovePriority movePriority;
    [SerializeField] protected IntNullable baseDamage;
    [SerializeField] protected IntNullable baseAccuracy;
    [SerializeField] protected int basePP;

    public abstract void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem);
    
    public abstract void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem);
    
    public string GetMoveName() { return moveName; }
    
    public string GetDescription() { return description; }
    
    public TerraType GetMoveType() {  return moveType; }
    
    public DamageType GetDamageType() {  return damageType; }
    
    public MovePriority GetMovePriority() {  return movePriority; }
    
    public IntNullable GetBaseDamage() { return baseDamage; }
    
    public IntNullable GetBaseAccuracy() { return baseAccuracy; }
    
    public int GetBasePP() { return basePP; }
}
