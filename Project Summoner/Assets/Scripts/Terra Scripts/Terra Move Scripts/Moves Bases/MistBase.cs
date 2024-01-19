using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Mist")]
public class MistBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new MistAction();
    }
}

public class MistAction : TerraMoveAction
{
    public MistAction() {}
    
    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (!directAttackLog.GetAttackerPosition().AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Stat Resistance"), battleSystem))
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}