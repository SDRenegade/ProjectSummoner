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

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += IncreaseEvasiveness;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= IncreaseEvasiveness;
    }

    public void IncreaseEvasiveness(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddEvasivenessModifier(EVASIVENESS_MODIFIER);
    }
}
