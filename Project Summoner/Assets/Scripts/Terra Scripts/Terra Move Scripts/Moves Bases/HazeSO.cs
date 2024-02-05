using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Haze")]
public class HazeSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Haze(terraAttack, this);
    }
}

public class Haze : TerraMoveBase
{
    public Haze(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        attackerPosition.ResetStatStages();
        Debug.Log(BattleDialog.ResetStatStagesMsg(attackerPosition.GetTerra()));
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}