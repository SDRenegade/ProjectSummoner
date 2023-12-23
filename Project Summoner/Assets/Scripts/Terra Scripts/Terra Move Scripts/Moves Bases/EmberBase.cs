using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Ember")]
public class EmberBase : TerraMoveBase
{
    private static readonly int BURN_CHANCE = 20;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();

        bool isBurned = BURN_CHANCE >= Random.Range(0, 100);
        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() == null && isBurned) {
            defenderPosition.GetTerra().GetStatusEffect().SetStatusEffectBase(SODatabase.GetInstance().GetStatusEffectByName("Burn"), defenderPosition.GetTerra());
            defenderPosition.GetTerra().GetStatusEffect().AddStatusEffectBattleActoin(battleSystem, defenderPosition.GetTerra());
            BattleDialog.BurnInflictedMsg(defenderPosition.GetTerra());
        }
    }
}
