using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveDatabase", menuName = "Database/Terra Move Database")]
public class TerraMoveDatabase : ScriptableObject
{
    [SerializeField] private List<TerraMoveBase> terraMoveList;

    public List<TerraMoveBase> GetTerraMoveList() { return terraMoveList; }
}
