using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Crab Hammer")]
public class CrabHammerBase : TerraMoveBase
{
    private static readonly float CRIT_MODIFIER = 1.2f;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem)
    {
        terraAttackParams.SetCritModifier(terraAttackParams.GetCritModifier() * CRIT_MODIFIER);
    }

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem) {}
}
