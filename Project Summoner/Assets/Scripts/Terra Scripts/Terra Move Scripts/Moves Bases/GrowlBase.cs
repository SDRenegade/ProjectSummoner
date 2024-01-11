using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Growl")]
public class GrowlBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new GrowlAction();
    }
}

public class GrowlAction : TerraMoveAction
{
    public GrowlAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();
        defenderPosition.SetAttackStage(StatStagesExtension.DecrementStage(defenderPosition.GetAttackStage()));
        Debug.Log(BattleDialog.StatStageDecrementMsg(defenderPosition.GetTerra(), defenderPosition.GetAttackStage(), -1));
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}