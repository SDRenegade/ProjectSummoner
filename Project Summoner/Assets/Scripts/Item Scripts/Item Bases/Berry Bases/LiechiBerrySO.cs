using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Liechi Berry")]
public class LiechiBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LiechiBerry(this);
    }
}

public class LiechiBerry : ItemBase
{
    private static readonly Stats TARGET_STAT = Stats.ATK;
    private static readonly int NUMBER_OF_STAGES = 1;
    private static readonly float PERCENT_HEALTH_ACTIVATION = 1/3f;

    private TerraBattlePosition terraBattlePosition;

    public LiechiBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += RaiseStatAfterTerraAttackDamageProke;
        battleSystem.OnPostTerraDamaged += RaiseStatAfterOtherDamageProke;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= RaiseStatAfterTerraAttackDamageProke;
        battleSystem.OnPostTerraDamaged -= RaiseStatAfterOtherDamageProke;
    }

    private void RaiseStatAfterTerraAttackDamageProke(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() > terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_ACTIVATION)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().ChanageTerraStat(terraBattlePosition, TARGET_STAT, NUMBER_OF_STAGES);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }

    private void RaiseStatAfterOtherDamageProke(object sender, TerraDamagedEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() > terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_ACTIVATION)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().ChanageTerraStat(terraBattlePosition, TARGET_STAT, NUMBER_OF_STAGES);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}
