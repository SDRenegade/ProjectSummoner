using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerraSavable
{
    [SerializeField] private string terraBaseName;
    [SerializeField] private string terraNickname;
    [SerializeField] private int level;
    [SerializeField] private List<TerraMoveSavable> savableMoves;
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int currentHP;

    public TerraSavable(Terra terra)
    {
        terraBaseName = terra.GetTerraBase().GetSpeciesName();
        terraNickname = terra.GetTerraNickname();
        level = terra.GetLevel();
        savableMoves = new List<TerraMoveSavable>();
        for(int i = 0; i < terra.GetMoves().Count; i++)
            savableMoves.Add(new TerraMoveSavable(terra.GetMoves()[i]));
        statusEffect = terra.GetStatusEffect();
        currentHP = terra.GetCurrentHP();
    }

    public string GetTerraBaseName() { return terraBaseName; }

    public string GetTerraNickname() {  return terraNickname; }

    public int GetLevel() { return level; }

    public List<TerraMoveSavable> GetSavableMoves() {  return savableMoves; }

    public StatusEffect GetStatusEffect() {  return statusEffect; }

    public int GetCurrentHP() {  return currentHP; }
}
