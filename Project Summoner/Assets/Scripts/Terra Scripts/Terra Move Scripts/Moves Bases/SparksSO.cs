using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sparks")]
public class SparksSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Sparks(terraAttack, this);
    }
}

public class Sparks : TerraMoveBase
{
    private static readonly float PARALYZE_CHANCE = 0.4f;

    public Sparks(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        if (PARALYZE_CHANCE >= Random.Range(0f, 1f))
            battleSystem.AddStatusEffect(defenderPosition, SODatabase.GetInstance().GetStatusEffectByName("Paralysis"));
    }

    public override void AddMoveListeners(BattleSystem battleSystem) {}

    public override void RemoveMoveListeners(BattleSystem battleSystem) {}
}