using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/FlashFreeze")]
public class FlashFreezeBase : TerraMoveBase
{
    private static readonly int FREEZE_CHANCE = 20;

    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        bool isFrozen = FREEZE_CHANCE > Random.Range(0, 100);
        if (targetTerraPosition.GetTerra().GetStatusEffect() == null && isFrozen) {
            attackingTerraPosition.GetTerra().SetStatusEffect(new FreezeStatusEffect(attackingTerraPosition.GetTerra()));
            attackingTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            targetTerraPosition.GetTerra().SetStatusEffect(new FreezeStatusEffect(targetTerraPosition.GetTerra()));
            targetTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            BattleDialog.FreezeInflictedMsg(targetTerraPosition.GetTerra());
        }
    }
}
