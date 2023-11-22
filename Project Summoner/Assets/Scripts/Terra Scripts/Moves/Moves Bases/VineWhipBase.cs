using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/VineWhip")]
public class VineWhipBase : TerraMoveBase
{
    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}
}
