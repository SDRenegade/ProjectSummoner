using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class ChoiceBandSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        throw new System.NotImplementedException();
    }
}

public class ChoiceBand : ItemBase
{
    private static readonly float DAMAGE_MODIFIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;
    private int? chosenMoveIndex;

    public ChoiceBand(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        chosenMoveIndex = null;

        battleSystem.OnDirectAttack += ApplyDamageModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= ApplyDamageModifier;
    }

    public void ApplyDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (chosenMoveIndex == null)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER);
    }
}