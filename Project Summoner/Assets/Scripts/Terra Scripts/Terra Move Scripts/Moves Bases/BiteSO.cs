using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bite")]
public class BiteSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Bite(terraAttack, this);
    }
}

public class Bite : TerraMoveBase
{
    private static readonly float FLINCH_CHANCE = 1/5f;

    public Bite(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        battleSystem.RollForVolatileStatusEffect(
            directAttackLog.GetAttackerPosition(),
            directAttackLog.GetDefenderPosition(),
            SODatabase.GetInstance().GetVolatileStatusEffectByName("Flinched"),
            FLINCH_CHANCE);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}