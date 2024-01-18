using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VolatileStatusEffectBase : ScriptableObject
{
    [SerializeField] protected string statusName;
    [SerializeField] [TextArea] protected string description;

    public VolatileStatusEffectBase() {}

    public abstract BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition);

    public string GetStatusName() { return statusName; }

    public string GetDescription() { return description; }
}
