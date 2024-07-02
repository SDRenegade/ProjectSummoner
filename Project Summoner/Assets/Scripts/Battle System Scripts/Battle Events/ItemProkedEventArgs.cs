using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProkedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra terra;
    private ItemSO item;

    public ItemProkedEventArgs(TerraBattlePosition terraBattlePosition, ItemSO item, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        terra = terraBattlePosition.GetTerra();
        this.item = item;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GetTerra() { return terra; }

    public ItemSO GetItemSO() { return item; }
}
