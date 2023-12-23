using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bite")]
public class BiteBase : TerraMoveBase
{
    private static readonly float FLINCH_CHANCE = 1/4f;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        if (Random.Range(0f, 1f) > FLINCH_CHANCE)
            return;

        TerraBattlePosition defenderPosition = terraAttackLogList[0].GetDefenderPosition();
        foreach(TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if(terraAttack.GetAttackerPosition() == defenderPosition) {
                terraAttack.SetFlinched(true);
            }
        }
    }
}
