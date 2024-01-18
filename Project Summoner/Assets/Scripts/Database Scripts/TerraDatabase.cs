using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraDatabase", menuName = "Database/Terra Database")]
public class TerraDatabase : ScriptableObject
{
    [SerializeField] private List<TerraBase> terraBaseList;

    public List<TerraBase> GetTerraBases() { return terraBaseList; }
}
