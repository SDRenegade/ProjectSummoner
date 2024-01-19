using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Adamant Orb")]
public class AdamantOrbBase : ItemBase
{
    private static readonly string TERRA_SPECIES = "Flaregon";
    private static readonly TerraType EMPOWERED_MOVE_TYPE = TerraType.NATURE;
    private static readonly float DAMAGE_MULTIPLIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += FireTypeDamageMultipler;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= FireTypeDamageMultipler;
    }

    public void FireTypeDamageMultipler(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition().GetTerra().GetTerraBase().GetSpeciesName() != TERRA_SPECIES)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveBase().GetMoveType() != EMPOWERED_MOVE_TYPE)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MULTIPLIER);
    }
}