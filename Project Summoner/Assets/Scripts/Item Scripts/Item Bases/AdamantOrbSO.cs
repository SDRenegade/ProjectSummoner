using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Adamant Orb")]
public class AdamantOrbSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new AdamantOrb(this);
    }
}

public class AdamantOrb : ItemBase
{
    private static readonly string TERRA_SPECIES = "Flaregon";
    private static readonly TerraType EMPOWERED_MOVE_TYPE = TerraType.NATURE;
    private static readonly float DAMAGE_MULTIPLIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;

    public AdamantOrb(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += FireTypeDamageMultipler;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= FireTypeDamageMultipler;
    }

    public void FireTypeDamageMultipler(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition().GetTerra().GetTerraBase().GetSpeciesName() != TERRA_SPECIES)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveSO().GetTerraType() != EMPOWERED_MOVE_TYPE)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MULTIPLIER);
    }
}
