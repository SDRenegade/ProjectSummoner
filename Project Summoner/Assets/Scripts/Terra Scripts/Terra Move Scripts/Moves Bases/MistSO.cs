using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Mist")]
public class MistSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Mist(terraAttack, this);
    }
}

public class Mist : TerraMoveBase
{
    public Mist(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Stat Resistance")))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}