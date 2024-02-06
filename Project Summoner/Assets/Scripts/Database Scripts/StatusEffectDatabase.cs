using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDatabase", menuName = "Database/Status Effect Database")]
public class StatusEffectDatabase : ScriptableObject
{
    [SerializeField] private List<StatusEffectSO> statusEffectList;

    public List<StatusEffectSO> GetStatusEffectList() { return statusEffectList; }
}
