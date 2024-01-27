using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase
{
    protected ItemSO itemSO;

    public ItemBase(ItemSO itemSO)
    {
        this.itemSO = itemSO;
    }

    public abstract void OnOverworldUse();

    public abstract void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem);

    public abstract void RemoveBattleActions(BattleSystem battleSystem);

    public ItemSO GetItemSO() { return itemSO; }

    public override string ToString() { return itemSO.GetItemName(); }
}
