using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Flash Freeze")]
public class FlashFreezeBase : TerraMoveBase
{
    private static readonly float FREEZE_CHANCE = 0.2f;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = terraAttackTerraList[0].GetAttackerPosition();
        TerraBattlePosition defenderPosition = terraAttackTerraList[0].GetDefenderPosition();

        bool hasFreezeProked = FREEZE_CHANCE >= Random.Range(0f, 1f);
        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() == null && hasFreezeProked) {
            defenderPosition.GetTerra().GetStatusEffect().SetStatusEffectBase(SODatabase.GetInstance().GetStatusEffectByName("Freeze"), defenderPosition.GetTerra());
            defenderPosition.GetTerra().GetStatusEffect().AddStatusEffectBattleActoin(battleSystem, defenderPosition.GetTerra());
            BattleDialog.FreezeInflictedMsg(defenderPosition.GetTerra());
        }
    }
}
