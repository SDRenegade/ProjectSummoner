using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Disable")]
public class DisableBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DisableAction();
    }
}

public class DisableAction : TerraMoveAction
{
    public DisableAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        bool isAdded = directAttackLog.GetDefenderPosition().AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Disabled"), battleSystem);
        if (isAdded)
            Debug.Log(BattleDialog.DisableActiveMsg(directAttackLog.GetDefenderPosition().GetTerra()));
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}