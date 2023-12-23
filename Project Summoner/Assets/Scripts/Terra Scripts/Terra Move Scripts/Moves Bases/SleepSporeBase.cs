using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sleep Spore")]
public class SleepSporeBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();

        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() == null) {
            defenderPosition.GetTerra().GetStatusEffect().SetStatusEffectBase(SODatabase.GetInstance().GetStatusEffectByName("Sleep"), defenderPosition.GetTerra());
            defenderPosition.GetTerra().GetStatusEffect().AddStatusEffectBattleActoin(battleSystem, defenderPosition.GetTerra());
            BattleDialog.SleepInflictedMsg(defenderPosition.GetTerra());
        }
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }
}
