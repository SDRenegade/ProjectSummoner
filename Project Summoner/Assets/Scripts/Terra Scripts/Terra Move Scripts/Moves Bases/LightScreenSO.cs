using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Light Screen")]
public class LightScreenSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new LightScreen(terraAttack, this);
    }
}

public class LightScreen : TerraMoveBase
{
    public LightScreen(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Special Barrier")))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}