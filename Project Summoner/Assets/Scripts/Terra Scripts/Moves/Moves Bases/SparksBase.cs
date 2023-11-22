using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sparks")]
public class SparksBase : TerraMoveBase
{
    private static readonly int PARALYZE_CHANCE = 20;

    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        bool isParalyzed = PARALYZE_CHANCE >= Random.Range(0, 100);
        if (targetTerraPosition.GetTerra().GetStatusEffect() == null && isParalyzed) {
            targetTerraPosition.GetTerra().SetStatusEffect(new ParalysisStatusEffect(targetTerraPosition.GetTerra()));
            targetTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            Debug.Log(BattleDialog.ParalysisInflictedMsg(targetTerraPosition.GetTerra()));
        }
    }
}
