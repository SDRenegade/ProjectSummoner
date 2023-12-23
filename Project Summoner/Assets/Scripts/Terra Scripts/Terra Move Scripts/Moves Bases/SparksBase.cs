using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sparks")]
public class SparksBase : TerraMoveBase
{
    private static readonly int PARALYZE_CHANCE = 20;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();

        bool isParalyzed = PARALYZE_CHANCE >= Random.Range(0, 100);
        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() == null && isParalyzed) {
            defenderPosition.GetTerra().GetStatusEffect().SetStatusEffectBase(SODatabase.GetInstance().GetStatusEffectByName("Paralysis"), defenderPosition.GetTerra());
            defenderPosition.GetTerra().GetStatusEffect().AddStatusEffectBattleActoin(battleSystem, defenderPosition.GetTerra());
            Debug.Log(BattleDialog.ParalysisInflictedMsg(defenderPosition.GetTerra()));
        }
    }
}
