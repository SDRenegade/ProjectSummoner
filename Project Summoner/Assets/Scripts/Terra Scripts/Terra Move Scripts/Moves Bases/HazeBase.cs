using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Haze")]
public class HazeBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) { }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = terraAttackLogList[0].GetAttackerPosition();

        attackerPosition.ResetStatStages();
        Debug.Log(BattleDialog.ResetStatStagesMsg(attackerPosition.GetTerra()));
    }
}
