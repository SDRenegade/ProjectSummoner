using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bite")]
public class BiteBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new BiteAction();
    }
}

public class BiteAction : TerraMoveAction
{
    private static readonly float FLINCH_CHANCE = 1/5f;

    public BiteAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (Random.Range(0f, 1f) > FLINCH_CHANCE)
            return;

        battleSystem.AddVolatileStatusEffect(directAttackLog.GetDefenderPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Flinched"));
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}