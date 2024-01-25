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

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddDamageModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddDamageModifier;
    }

    public void AddDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        TerraMoveBase moveBase = eventArgs.GetDirectAttackParams().GetMove().GetMoveBase();
        Terra defendingTerra = eventArgs.GetDirectAttackParams().GetDefenderPosition().GetTerra();
        if (moveBase.GetMoveType().GetTypeEffectivenessModifier(defendingTerra.GetTerraBase().GetTerraTypes()) < TerraTypeExtension.GetEffectivenessTypeValue(EffectivenessTypes.SUPER))
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER);
    }
}
