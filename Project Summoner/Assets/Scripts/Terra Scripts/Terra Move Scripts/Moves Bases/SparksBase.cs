using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sparks")]
public class SparksBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SparksAction();
    }
}

public class SparksAction : TerraMoveAction
{
    private static readonly int PARALYZE_CHANCE = 20;

    public SparksAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        bool isParalyzed = PARALYZE_CHANCE >= Random.Range(0, 100);
        if (!defenderPosition.GetTerra().HasStatusEffect() && isParalyzed) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Paralysis"), battleSystem);
            Debug.Log(BattleDialog.ParalysisInflictedMsg(defenderPosition.GetTerra()));
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}