using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraBase", menuName = "Terra/Create new TerraBase")]
public class TerraBase : ScriptableObject
{
    public const int MAX_CATCH_RATE = 255;

    [SerializeField] private string speciesName;
    [SerializeField] [TextArea] private string logEntry;
    [SerializeField] private List<TerraType> terraTypes;
    [SerializeField] private List<LearnedMove> naturalMovePool;
    [SerializeField] private List<TerraMoveSO> teachableMovePool;

    [SerializeField] private int baseHP;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefence;
    [SerializeField] private int baseSpAttack;
    [SerializeField] private int baseSpDefence;
    [SerializeField] private int baseSpeed;
    [SerializeField] [Range(0, MAX_CATCH_RATE)] private int baseCatchRate;

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
    public int GetBaseCatchRate() { return baseCatchRate; }
    public GameObject GetTerraGameObject() {  return terraGameObject; }

}
