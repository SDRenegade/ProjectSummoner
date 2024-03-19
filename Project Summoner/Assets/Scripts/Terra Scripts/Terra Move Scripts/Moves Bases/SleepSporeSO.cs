using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sleep Spore")]
public class SleepSporeSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SleepSpore(terraAttack, this);
    }
}

public class SleepSpore : TerraMoveBase
{
    public SleepSpore(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        if (!battleSystem.AddStatusEffect(defenderPosition, SODatabase.GetInstance().GetStatusEffectByName("Sleep")))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddMoveListeners(BattleSystem battleSystem) {}

    public override void RemoveMoveListeners(BattleSystem battleSystem) {}
}