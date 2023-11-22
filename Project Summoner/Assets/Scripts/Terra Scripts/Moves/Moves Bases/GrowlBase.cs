using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Growl")]
public class GrowlBase : TerraMoveBase
{
    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        targetTerraPosition.SetAttackStage(StatStagesExtension.DecrementStage(targetTerraPosition.GetAttackStage()));
        Debug.Log(BattleDialog.StatStageDecrementMsg(targetTerraPosition.GetTerra(), targetTerraPosition.GetAttackStage(), -1));
    }
}
