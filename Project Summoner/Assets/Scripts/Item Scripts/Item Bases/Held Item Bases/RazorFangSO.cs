using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Razor Fang")]
public class RazorFangSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new RazorFang(this);
    }
}

public class RazorFang : ItemBase
{
    private static readonly float FLINCH_CHANCE_MODIFIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;

    public RazorFang(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnVolatileStatusEffectRoll += AddFlinchChanceModifier;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnVolatileStatusEffectRoll -= AddFlinchChanceModifier;
    }

    public void AddFlinchChanceModifier(object sender, VolatileStatusEffectRollEventArgs eventArgs)
    {
        if (eventArgs.GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetVolatileStatusEffect() != SODatabase.GetInstance().GetVolatileStatusEffectByName("Flinched"))
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.AddRollOddsModifier(FLINCH_CHANCE_MODIFIER);
    }
}