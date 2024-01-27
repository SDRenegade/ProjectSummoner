using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Life Orb")]
public class LifeOrbSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LifeOrb(this);
    }
}

public class LifeOrb : ItemBase
{
    private static readonly float DAMAGE_MODIFIER = 1.25f;
    private static readonly float PERCENT_MAX_HEALTH_RECOIL = 1/16f;

    private TerraBattlePosition terraBattlePosition;

    public LifeOrb(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += ApplyDamageModifier;
        battleSystem.OnPostAttack += ItemRecoilDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= ApplyDamageModifier;
        battleSystem.OnPostAttack -= ItemRecoilDamage;
    }

    public void ApplyDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER);
    }

    public void ItemRecoilDamage(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        int recoilDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOIL);
        eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, recoilDamage);
    }
}
