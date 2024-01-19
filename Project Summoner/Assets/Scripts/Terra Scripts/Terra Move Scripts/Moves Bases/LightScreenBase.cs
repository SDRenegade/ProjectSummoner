using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Light Screen")]
public class LightScreenBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new LightScreenAction();
    }
}

public class LightScreenAction : TerraMoveAction
{
    public LightScreenAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();
        bool isSuccessfullyAdded = attackerPosition.AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Special Barrier"), battleSystem);
        if (!isSuccessfullyAdded)
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}