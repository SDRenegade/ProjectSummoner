using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Substitute")]
public class SubstituteBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SubstituteAction();
    }
}

public class SubstituteAction : TerraMoveAction
{
    public SubstituteAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!directAttackLog.GetAttackerPosition().AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Substituted"), battleSystem))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}