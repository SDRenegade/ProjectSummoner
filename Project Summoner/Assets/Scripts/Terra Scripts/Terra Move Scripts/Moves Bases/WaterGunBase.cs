using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Water Gun")]
public class WaterGunBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) { }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem) {}
}
