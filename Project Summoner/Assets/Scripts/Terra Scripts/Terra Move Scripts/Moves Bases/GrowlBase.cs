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
    private static readonly int ATK_MODIFICATION = -1;

    public GrowlAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        //*** Stat Change Event ***
        StatChangeEventArgs statChangeEventArgs = battleSystem.InvokeOnStatChange(defenderPosition, Stats.ATK, ATK_MODIFICATION);

        if(!statChangeEventArgs.IsCanceled()) {
            defenderPosition.SetStatStage(Stats.ATK, StatStagesExtension.ChangeStatStage(defenderPosition.GetStatStage(Stats.ATK), statChangeEventArgs.GetModification()));
            Debug.Log(BattleDialog.StatStageChangeMsg(defenderPosition.GetTerra(), Stats.ATK, defenderPosition.GetStatStage(Stats.ATK), statChangeEventArgs.GetModification()));
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}