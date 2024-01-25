using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffectDatabase", menuName = "Database/Volatile Status Effect Database")]
public class VolatileStatusEffectDatabase : ScriptableObject
{
    [SerializeField] private List<VolatileStatusEffectSO> volatileStatusEffectList;

    public List<VolatileStatusEffectSO> GetVolatileStatusEffectList() { return volatileStatusEffectList; }
}
