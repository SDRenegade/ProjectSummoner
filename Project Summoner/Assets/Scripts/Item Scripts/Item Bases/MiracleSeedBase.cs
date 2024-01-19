using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Miracle Seed")]
public class MiracleSeedBase : ItemBase
{
    private static readonly TerraType EMPOWERED_TYPE = TerraType.NATURE;
    private static readonly float DAMAGE_MULTIPLIER = 1.2f;

    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += NatureTypeDamageMultipler;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= NatureTypeDamageMultipler;
    }

    public void NatureTypeDamageMultipler(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveBase().GetMoveType() != EMPOWERED_TYPE)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MULTIPLIER);
    }
}
