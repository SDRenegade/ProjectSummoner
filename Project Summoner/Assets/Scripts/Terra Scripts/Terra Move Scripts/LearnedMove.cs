using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearnedMove
{
    [SerializeField] private TerraMoveBase movebase;
    [SerializeField] private int levelLearned;

    public TerraMove GetMove() { return new TerraMove(movebase); }
    public int GetLevelLearned() {  return levelLearned; }
}
