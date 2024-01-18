using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffectDatabase", menuName = "Database/Volatile Status Effect Database")]
public class VolatileStatusEffectDatabase : ScriptableObject
{
    [SerializeField] private List<VolatileStatusEffectBase> volatileStatusEffectList;

    public List<VolatileStatusEffectBase> GetVolatileStatusEffectList() { return volatileStatusEffectList; }
}
