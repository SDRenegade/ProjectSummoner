using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraSpawn", menuName = "Terra/Create new TerraSpawn")]
public class TerraSpawnEntry : ScriptableObject
{
    [SerializeField] private TerraBase terraBase;
    [SerializeField] private int minLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int weight;

    public TerraBase GetTerraBase() { return terraBase; }
    public void SetTerraBase(TerraBase terraBase) {  this.terraBase = terraBase; }
    public int GetMinLevel() {  return minLevel; }
    public void SetMinLevel(int minLevel) { this.minLevel = minLevel; }
    public int GetMaxLevel() { return maxLevel; }
    public void SetMaxLevel(int maxLevel) { this.maxLevel = maxLevel; }
    public int GetWeight() { return weight; }
    public void SetWeight(int weight) {  this.weight = weight; }

}
