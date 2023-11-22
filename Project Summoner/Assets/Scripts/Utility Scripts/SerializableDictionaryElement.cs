using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionaryElement<TKey, TValue>
{
    private TKey key;
    private TValue value;

    public SerializableDictionaryElement(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }

    public TKey GetKey() { return key; }

    public void SetKey(TKey key) { this.key = key; }

    public TValue GetValue() { return value; }

    public void SetValue(TValue value) { this.value = value; }
}
