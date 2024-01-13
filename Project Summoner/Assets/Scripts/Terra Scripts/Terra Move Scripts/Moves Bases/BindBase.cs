using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bind")]
public class BindBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new BindAction();
    }
}

public class BindAction : TerraMoveAction
{
    public BindAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();
        bool isSuccessfullyAdded = defenderPosition.AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Bound"), battleSystem);
        if (!isSuccessfullyAdded)
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}