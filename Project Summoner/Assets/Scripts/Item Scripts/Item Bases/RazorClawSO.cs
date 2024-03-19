using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Razor Claw")]
public class RazorClawSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new RazorClaw(this);
    }
}

public class RazorClaw : ItemBase
{
    private static readonly float CRIT_MODIFIER = 1.05f;

    private TerraBattlePosition terraBattlePosition;

    public RazorClaw(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddCritModifier;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddCritModifier;
    }

    public void AddCritModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddCritModifier(CRIT_MODIFIER);
    }
}
