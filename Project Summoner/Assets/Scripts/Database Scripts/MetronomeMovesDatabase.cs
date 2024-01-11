using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MetronomeMoveDatabase", menuName = "Database/Metronome Move Database")]
public class MetronomeMovesDatabase : ScriptableObject
{
    [SerializeField] private List<TerraMoveBase> metronomeMoveList;

    public List<TerraMoveBase> GetMetronomeMoveList() { return metronomeMoveList; }
}
