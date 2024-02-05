using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Oran Berry")]
public class OranBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new OranBerry(this);
    }
}

public class OranBerry : ItemBase
{
    private static readonly int HEAL_AMT = 10;
    private static readonly float PERCENT_HEALTH_ACTIVATION = 0.4f;

    private TerraBattlePosition terraBattlePosition;

    public OranBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += AfterTerraAttackDamageProke;
        battleSystem.OnPostTerraDamaged += AfterOtherDamageProke;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
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
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, HEAL_AMT);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }

    private void AfterOtherDamageProke(object sender, TerraDamagedEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() > terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_HEALTH_ACTIVATION)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, HEAL_AMT);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}