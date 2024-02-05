using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bind")]
public class BindSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Bind(terraAttack, this);
    }
}

public class Bind : TerraMoveBase
{
    public Bind(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();
        bool isSuccessfullyAdded = battleSystem.AddVolatileStatusEffect(defenderPosition, SODatabase.GetInstance().GetVolatileStatusEffectByName("Bound"));
        if (!isSuccessfullyAdded)
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}