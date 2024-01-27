using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Focus Sash")]
public class FocusSashSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new FocusSash(this);
    }
}

public class FocusSash : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public FocusSash(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnTerraDamageByTerra += SurviveOneHitKO;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= SurviveOneHitKO;
    }

    public void SurviveOneHitKO(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() != terraBattlePosition.GetTerra().GetMaxHP())
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() < terraBattlePosition.GetTerra().GetMaxHP())
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackLog().SetDamage(terraBattlePosition.GetTerra().GetMaxHP() - 1);

        ConsumeOnUse(eventArgs.GetBattleSystem());
    }

    private void ConsumeOnUse(BattleSystem battleSystem)
    {
        Debug.Log(BattleDialog.ItemConsumedMsg(this));
        RemoveBattleActions(battleSystem);
        terraBattlePosition.GetTerra().SetHeldItem(null);
    }
}