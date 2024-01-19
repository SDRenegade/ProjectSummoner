using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/SleepSpore")]
public class SleepSporeBase : TerraMoveBase
{
    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        if (targetTerraPosition.GetTerra().GetStatusEffect() == null) {
            targetTerraPosition.GetTerra().SetStatusEffect(new SleepStatusEffect(targetTerraPosition.GetTerra()));
            targetTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            BattleDialog.SleepInflictedMsg(targetTerraPosition.GetTerra());
        }
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }
}
