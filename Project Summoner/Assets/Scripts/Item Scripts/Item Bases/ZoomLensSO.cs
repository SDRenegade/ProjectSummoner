using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Zoom Lens")]
public class ZoomLensSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new ZoomLens(this);
    }
}

public class ZoomLens : ItemBase
{
    private static readonly float ACCURACY_MODIFIER = 1.2f;

    private TerraBattlePosition terraBattlePosition;
    private bool isAttacked;

    public ZoomLens(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        isAttacked = false;
        battleSystem.OnDirectAttack += AddAccuracyModifier;
        battleSystem.OnDirectAttack += DetectTargetedAttack;
        battleSystem.OnEndOfTurn += EndOfTurnFlagReset;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddAccuracyModifier;
        battleSystem.OnDirectAttack -= DetectTargetedAttack;
        battleSystem.OnEndOfTurn -= EndOfTurnFlagReset;
    }

    public void DetectTargetedAttack(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;

        isAttacked = true;
    }

    public void AddAccuracyModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (!isAttacked)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddAccuracyModifier(ACCURACY_MODIFIER);
    }

    public void EndOfTurnFlagReset(object sender, BattleEventArgs eventArgs)
    {
        isAttacked = false;
    }
}
