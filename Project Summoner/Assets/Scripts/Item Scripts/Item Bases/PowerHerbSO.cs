using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Power Herb")]
public class PowerHerbSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new PowerHerb(this);
    }
}

public class PowerHerb : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public PowerHerb(ItemSO itemSO) : base(itemSO) {}
    
    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnAttackCharging += SkipChargeTurn;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackCharging -= SkipChargeTurn;
    }

    private void SkipChargeTurn(object sender, AttackChargingEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.SetCanceled(true);

        ConsumeOnUse(eventArgs.GetBattleSystem());
    }

    private void ConsumeOnUse(BattleSystem battleSystem)
    {
        Debug.Log(BattleDialog.ItemConsumedMsg(this));
        RemoveItemListeners(battleSystem);
        terraBattlePosition.GetTerra().SetHeldItem(null);
    }
}