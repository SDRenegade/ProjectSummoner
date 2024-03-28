using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Bright Powder")]
public class BrightPowderSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new BrightPowder(this);
    }
}

public class BrightPowder : ItemBase
{
    private static readonly float EVASIVENESS_MODIFIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;

    public BrightPowder(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += IncreaseEvasiveness;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= IncreaseEvasiveness;
    }

    public void IncreaseEvasiveness(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddEvasivenessModifier(EVASIVENESS_MODIFIER);
    }
}
