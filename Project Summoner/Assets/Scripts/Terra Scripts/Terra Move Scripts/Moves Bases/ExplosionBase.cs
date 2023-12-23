using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Explosion")]
public class ExplosionBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}
    
    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        terraAttackLogList[0].GetAttackerPosition().GetTerra().SetCurrentHP(0);
    }
}
