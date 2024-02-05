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
    private static readonly int PARALYZE_CHANCE = 20;

    public Sparks(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        bool isParalyzed = PARALYZE_CHANCE >= Random.Range(0, 100);
        if (!defenderPosition.GetTerra().HasStatusEffect() && isParalyzed) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Paralysis"), defenderPosition, battleSystem);
            Debug.Log(BattleDialog.ParalysisInflictedMsg(defenderPosition.GetTerra()));
        }
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}