using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Leftovers")]
public class LeftoversSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new Leftovers(this);
    }
}

public class Leftovers : ItemBase
{
    private static readonly float PERCENT_HEALTH_HEAL = 1/2f;

    private TerraBattlePosition terraBattlePosition;

    public Leftovers(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnEndOfTurn += HealthRegen;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= HealthRegen;
    }

    public void HealthRegen(object sender, BattleEventArgs eventArgs)
    {
        Debug.Log(BattleDialog.ItemProkedMsg(this));

        Terra terra = terraBattlePosition.GetTerra();
        int? healAmt = (int)(terra.GetMaxHP() * PERCENT_HEALTH_HEAL);
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, healAmt);
    }
}