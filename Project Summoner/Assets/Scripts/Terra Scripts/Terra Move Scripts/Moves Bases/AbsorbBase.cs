using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Absorb")]
public class AbsorbBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        Terra attacker = terraAttackLogList[0].GetAttackerPosition().GetTerra();

        if(terraAttackLogList[0].GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)terraAttackLogList[0].GetDamage() / 2f);
            attacker.SetCurrentHP(attacker.GetCurrentHP() + healthAbsorbed);
            Debug.Log(BattleDialog.HealthHealedMsg(attacker, healthAbsorbed));
        }
    }
}
