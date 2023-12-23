using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Growl")]
public class GrowlBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();
        defenderPosition.SetAttackStage(StatStagesExtension.DecrementStage(defenderPosition.GetAttackStage()));
        Debug.Log(BattleDialog.StatStageDecrementMsg(defenderPosition.GetTerra(), defenderPosition.GetAttackStage(), -1));
    }
}
