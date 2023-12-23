using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect Database", menuName = "Database/Status Effect Database")]
public class StatusEffectDatabase : ScriptableObject
{
    [SerializeField] private List<StatusEffectBase> statusEffectBaseList;

    public List<StatusEffectBase> GetStatusEffectBases() { return statusEffectBaseList; }
}
