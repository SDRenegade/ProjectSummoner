using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Decay")]
public class DecayBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();

        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() == null) {
            defenderPosition.GetTerra().GetStatusEffect().SetStatusEffectBase(SODatabase.GetInstance().GetStatusEffectByName("Blight"), defenderPosition.GetTerra());
            defenderPosition.GetTerra().GetStatusEffect().AddStatusEffectBattleActoin(battleSystem, defenderPosition.GetTerra());
            Debug.Log(BattleDialog.BlightInflictedMsg(defenderPosition.GetTerra()));
        }
    }
}
