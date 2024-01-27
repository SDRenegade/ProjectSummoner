using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Shell Bell")]
public class ShellBellSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new ShellBell(this);
    }
}

public class ShellBell : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_HEAL = 1/8f;

    private TerraBattlePosition terraBattlePosition;

    public ShellBell(ItemSO itemSO) : base(itemSO) {}
    
    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += PostAttackDamageHeal;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= PostAttackDamageHeal;
    }

    public void PostAttackDamageHeal(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;
        if (terraBattlePosition.GetTerra().GetCurrentHP() == terraBattlePosition.GetTerra().GetMaxHP())
            return;

        int healAmt = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_HEAL);
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, healAmt);
    }
}