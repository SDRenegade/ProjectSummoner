using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField, TextArea] private string description;
    [SerializeField, TextArea] private string itemProkeDialog;
    [SerializeField] private Sprite sprite;

    public abstract ItemBase CreateItemBase();

    public string GetItemName() { return itemName; }

    public string GetDescription() { return description; }

    public string GetItemProkeDialog() { return itemProkeDialog; }

    public Sprite GetSprite() { return sprite; }
}
