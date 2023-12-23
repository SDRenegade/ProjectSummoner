using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffectBase : ScriptableObject
{
    [SerializeField] protected string statusName;
    [SerializeField] [TextArea] protected string description;

    public StatusEffectBase() {}

    public abstract BattleAction CreateBattleAction(Terra terra);

    public string GetStatusName() { return statusName; }

    public string GetDescription() { return description; }
}
