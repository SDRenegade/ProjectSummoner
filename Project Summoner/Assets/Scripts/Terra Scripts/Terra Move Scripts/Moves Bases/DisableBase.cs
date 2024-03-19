using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Disable")]
public class DisableBase : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Disable(terraAttack, this);
    }
}

public class Disable : TerraMoveBase
{
    public Disable(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (battleSystem.AddVolatileStatusEffect(directAttackLog.GetDefenderPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Disabled")))
            Debug.Log(BattleDialog.DisableActiveMsg(directAttackLog.GetDefenderPosition().GetTerra()));
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddMoveListeners(BattleSystem battleSystem) {}

    public override void RemoveMoveListeners(BattleSystem battleSystem) {}
}