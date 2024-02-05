using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Protect")]
public class ProtectSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Protect(terraAttack, this);
    }
}

public class Protect : TerraMoveBase
{
    public Protect(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Protected")))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}