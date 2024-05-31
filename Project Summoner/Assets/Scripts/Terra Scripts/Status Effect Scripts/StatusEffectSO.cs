using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffectSO : ScriptableObject
{
    [SerializeField] protected string statusName;
    [SerializeField] [TextArea] protected string description;
    [SerializeField] protected float captureMultiplier;

    public StatusEffectSO() {}

    public abstract StatusEffectBase CreateStatusEffectInstance();

    public string GetStatusName() { return statusName; }

    public string GetDescription() { return description; }

    public float GetCaptureMultiplier() { return captureMultiplier; }

    public override string ToString() { return statusName; }
}
