using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraBase", menuName = "Terra/Create new TerraBase")]
public class TerraBase : ScriptableObject
{
    [SerializeField] private string speciesName;
    [SerializeField] [TextArea] private string logEntry;
    [SerializeField] private List<TerraType> terraTypes;
    [SerializeField] private List<LearnedMove> naturalMovePool;
    [SerializeField] private List<TerraMoveBase> teachableMovePool;

    [SerializeField] private int baseHP;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefence;
    [SerializeField] private int baseSpAttack;
    [SerializeField] private int baseSpDefence;
    [SerializeField] private int baseSpeed;

    [SerializeField] private GameObject terraGameObject;

    public string GetSpeciesName() { return speciesName; }
    public string GetLogEntry() {  return logEntry; }
    public List<TerraType> GetTerraTypes() { return terraTypes; }
    public List<LearnedMove> GetNaturalMovePool() { return naturalMovePool; }
    public int GetBaseHP() { return baseHP; }
    public int GetBaseAttack() { return baseAttack; }
    public int GetBaseDefence() { return baseDefence; }
    public int GetBaseSpAttack() {  return baseSpAttack; }
    public int GetBaseSpDefence() { return baseSpDefence; }
    public int GetBaseSpeed() { return baseSpeed; } 
    public GameObject GetTerraGameObject() {  return terraGameObject; }

}
