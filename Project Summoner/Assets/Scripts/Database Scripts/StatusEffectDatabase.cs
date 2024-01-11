using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDatabase", menuName = "Database/Status Effect Database")]
public class StatusEffectDatabase : ScriptableObject
{
    [SerializeField] private List<StatusEffectBase> statusEffectList;

    public List<StatusEffectBase> GetStatusEffectList() { return statusEffectList; }
}
