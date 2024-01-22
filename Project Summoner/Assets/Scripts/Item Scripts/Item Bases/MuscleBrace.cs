using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Muscle Brace")]
public class MuscleBrace : ItemBase
{
    private static readonly float PHYSICAL_DAMAGE_MODIFIER = 1.25f;

    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += AddPhysicalDamageModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddPhysicalDamageModifier;
    }

    public void AddPhysicalDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveBase().GetDamageType() != DamageType.PHYSICAL)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(PHYSICAL_DAMAGE_MODIFIER);
    }
}
