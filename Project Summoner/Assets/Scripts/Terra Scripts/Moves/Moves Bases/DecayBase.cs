using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Decay")]
public class DecayBase : TerraMoveBase
{
    public override void PreAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem) {}

    public override void PostAttackAbility(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        if (targetTerraPosition.GetTerra().GetStatusEffect() == null) {
            targetTerraPosition.GetTerra().SetStatusEffect(new BlightStatusEffect(targetTerraPosition.GetTerra()));
            targetTerraPosition.GetTerra().GetStatusEffect().AddBattleEvent(battleSystem);
            Debug.Log(BattleDialog.BlightInflictedMsg(targetTerraPosition.GetTerra()));
        }
    }
}
