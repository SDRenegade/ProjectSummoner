using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] [TextArea] private string description;

    public abstract ItemBase CreateItemBase();

    public string GetItemName() { return itemName; }

    public string GetDescription() { return description; }
}
