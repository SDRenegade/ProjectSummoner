using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Terra Database", menuName = "Database/Terra Database")]
public class TerraDatabase : ScriptableObject
{
    [SerializeField] private List<TerraBase> terraBases;

    public List<TerraBase> GetTerraBases() { return terraBases; }
}
