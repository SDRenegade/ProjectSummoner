using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Muscle Brace")]
public class MuscleBraceSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new MuscleBrace(this);
    }
}

public class MuscleBrace : ItemBase
{
    private static readonly float PHYSICAL_DAMAGE_MODIFIER = 1.25f;

    private TerraBattlePosition terraBattlePosition;

    public MuscleBrace(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddPhysicalDamageModifier;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddPhysicalDamageModifier;
    }

    public void AddPhysicalDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveSO().GetDamageType() != DamageType.PHYSICAL)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(PHYSICAL_DAMAGE_MODIFIER);
    }
}