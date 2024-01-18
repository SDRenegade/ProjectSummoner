using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntNullable
{
    [SerializeField] private int value;
    [SerializeField] private bool isNull;

    public int GetValue() { return value; }

    public void SetValue(int value) { this.value = value; }

    public bool IsNull() { return isNull; }
    
    public void SetNull(bool isNull) { this.isNull = isNull; }
}
