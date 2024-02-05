using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MetronomeMoveDatabase", menuName = "Database/Metronome Move Database")]
public class MetronomeMovesDatabase : ScriptableObject
{
    [SerializeField] private List<TerraMoveSO> metronomeMoveList;

    public List<TerraMoveSO> GetMetronomeMoveList() { return metronomeMoveList; }
}
