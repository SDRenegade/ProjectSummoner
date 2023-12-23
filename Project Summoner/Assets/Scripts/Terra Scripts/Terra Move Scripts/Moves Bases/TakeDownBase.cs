using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Take Down")]
public class TakeDownBase : TerraMoveBase
{
    public static readonly float RECOIL_DAMAGE_PERCENT = 1/10f;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) { }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        Terra attacker = terraAttackLogList[0].GetAttackerPosition().GetTerra();

        int recoilDamage = (int)(attacker.GetMaxHP() * RECOIL_DAMAGE_PERCENT);
        attacker.TakeDamage(recoilDamage);
        Debug.Log(BattleDialog.RecoilDamageMsg(attacker, recoilDamage));
    }
}
