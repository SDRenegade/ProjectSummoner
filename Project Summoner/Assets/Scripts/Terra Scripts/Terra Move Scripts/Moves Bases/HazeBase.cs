using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Haze")]
public class HazeBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new HazeAction();
    }
}

public class HazeAction : TerraMoveAction
{
    public HazeAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        attackerPosition.ResetStatStages();
        Debug.Log(BattleDialog.ResetStatStagesMsg(attackerPosition.GetTerra()));
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}