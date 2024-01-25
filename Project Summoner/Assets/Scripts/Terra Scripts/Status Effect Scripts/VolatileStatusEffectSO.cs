using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VolatileStatusEffectSO : ScriptableObject
{
    [SerializeField] protected string statusName;
    [SerializeField] [TextArea] protected string description;

    public abstract VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition);

    public string GetStatusName() { return statusName; }

    public string GetDescription() { return description; }
}
