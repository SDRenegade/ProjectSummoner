using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Lax Incense")]
public class LaxIncenseSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LaxIncense(this);
    }
}

public class LaxIncense : ItemBase
{
    private static readonly float LOWERED_CRIT_MODIFIER = 0.95f;

    private TerraBattlePosition terraBattlePosition;

    public LaxIncense(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += DecreaseEnemyCritChance;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= DecreaseEnemyCritChance;
    }

    public void DecreaseEnemyCritChance(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddCritModifier(LOWERED_CRIT_MODIFIER);
    }
}
