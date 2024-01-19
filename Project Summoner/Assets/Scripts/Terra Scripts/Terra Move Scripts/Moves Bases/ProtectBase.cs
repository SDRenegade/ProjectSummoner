using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Protect")]
public class ProtectBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new ProtectAction();
    }
}

public class ProtectAction : TerraMoveAction
{
    public ProtectAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!directAttackLog.GetAttackerPosition().AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Protected"), battleSystem))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}