using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Wide Lens")]
public class WideLensSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new WideLens(this);
    }
}

public class WideLens : ItemBase
{
    private static readonly float ACCURACY_MODIFIER = 1.1f;

    private TerraBattlePosition terraBattlePosition;

    public WideLens(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddAccuracyModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddAccuracyModifier;
    }

    public void AddAccuracyModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddAccuracyModifier(ACCURACY_MODIFIER);
    }
}
