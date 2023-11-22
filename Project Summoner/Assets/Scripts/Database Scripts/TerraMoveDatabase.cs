using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Terra Move Database", menuName = "Database/Terra Move Database")]
public class TerraMoveDatabase : ScriptableObject
{
    [SerializeField] private List<TerraMoveBase> terraMoves;

    public List<TerraMoveBase> GetTerraMoves() { return terraMoves; }
}
