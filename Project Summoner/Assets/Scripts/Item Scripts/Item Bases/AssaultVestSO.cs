using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Assault Vest")]
public class AssaultVestSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new AssaultVest(this);
    }
}

public class AssaultVest : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_DAMAGE = 1/8f;

    private TerraBattlePosition terraBattlePosition;

    public AssaultVest(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += PostAttackDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= PostAttackDamage;
    }

    public void PostAttackDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        int damage = (int)(eventArgs.GetDirectAttackLog().GetAttackerPosition().GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_DAMAGE);
        eventArgs.GetBattleSystem().DamageTerra(eventArgs.GetDirectAttackLog().GetAttackerPosition(), damage);
    }
}