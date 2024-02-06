using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffectSO : ScriptableObject
{
    [SerializeField] protected string statusName;
    [SerializeField] [TextArea] protected string description;

    public StatusEffectSO() {}

    public abstract StatusEffectBase CreateStatusEffectInstance();

    public string GetStatusName() { return statusName; }

    public string GetDescription() { return description; }

    public override string ToString() { return statusName; }
}
