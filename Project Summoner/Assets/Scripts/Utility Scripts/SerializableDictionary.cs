using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableDictionaryElement<TKey, TValue>> elements;

    public SerializableDictionary()
    {
        elements = new List<SerializableDictionaryElement<TKey, TValue>>();
    }

    public void OnAfterDeserialize()
    {
        Clear();

        foreach(var element in elements)
            this.Add(element.GetKey(), element.GetValue());
    }

    public void OnBeforeSerialize()
    {
        elements.Clear();

        foreach(var pair in this)
            elements.Add(new SerializableDictionaryElement<TKey, TValue>(pair.Key, pair.Value));
    }
}
