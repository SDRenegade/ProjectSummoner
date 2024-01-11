using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bind")]
public class BindBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new BindAction(terraAttack);
    }
}

public class BindAction : TerraMoveAction
{
    private TerraAttack terraAttack;

    public BindAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

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