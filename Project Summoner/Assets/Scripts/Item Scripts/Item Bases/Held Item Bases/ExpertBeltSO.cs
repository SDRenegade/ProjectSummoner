using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Expert Belt")]
public class ExpertBeltSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new ExpertBelt(this);
    }
}

public class ExpertBelt : ItemBase
{
    private static readonly float DAMAGE_MODIFIER = 1.2f;

    private TerraBattlePosition terraBattlePosition;

    public ExpertBelt(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddDamageModifier;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddDamageModifier;
    }

    public void AddDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        TerraMoveSO moveBase = eventArgs.GetDirectAttackParams().GetMove().GetMoveSO();
        Terra defendingTerra = eventArgs.GetDirectAttackParams().GetDefenderPosition().GetTerra();
        if (moveBase.GetTerraType().GetTypeEffectivenessModifier(defendingTerra.GetTerraBase().GetTerraTypes()) < TerraTypeExtension.GetEffectivenessTypeValue(EffectivenessTypes.SUPER))
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER);
    }
}