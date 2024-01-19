using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] [TextArea] private string description;

    public virtual void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public virtual void RemoveBattleActions(BattleSystem battleSystem) {}

    public abstract void OnOverworldUse();

    public string GetItemName() { return  itemName; }

    public string GetDescription() { return description; }
}
