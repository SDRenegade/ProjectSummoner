using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Sitrus Berry")]
public class SitrusBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new SitrusBerry(this);
    }
}

public class SitrusBerry : ItemBase
{
    private static readonly float PERCENT_HEALTH_HEAL = 1/4f;
    private static readonly float PERCENT_HEALTH_ACTIVATION = 0.4f;

    private TerraBattlePosition terraBattlePosition;

    public SitrusBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += AfterTerraAttackDamageProke;
        battleSystem.OnPostTerraDamaged += AfterOtherDamageProke;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= AfterTerraAttackDamageProke;
        battleSystem.OnPostTerraDamaged -= AfterOtherDamageProke;
    }

    private void AfterTerraAttackDamageProke(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() > terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_ACTIVATION)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_HEAL));

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }

    private void AfterOtherDamageProke(object sender, TerraDamagedEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() > terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_ACTIVATION)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_HEAL));

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}