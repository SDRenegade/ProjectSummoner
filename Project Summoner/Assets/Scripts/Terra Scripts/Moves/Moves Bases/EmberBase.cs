using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Ember")]
public class EmberBase : TerraMoveBase
{
    private static readonly int BURN_CHANCE = 20;

    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        bool isBurned = BURN_CHANCE >= Random.Range(0, 100);
        if (targetTerraPosition.GetTerra().GetStatusEffect() == null && isBurned) {
            targetTerraPosition.GetTerra().SetStatusEffect(new BurnStatusEffect(targetTerraPosition.GetTerra()));
            targetTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            BattleDialog.BurnInflictedMsg(targetTerraPosition.GetTerra());
        }
    }
}
