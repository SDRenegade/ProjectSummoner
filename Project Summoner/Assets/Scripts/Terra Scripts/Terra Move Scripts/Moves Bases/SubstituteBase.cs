using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Substitute")]
public class SubstituteBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SubstituteAction(terraAttack);
    }
}

public class SubstituteAction : TerraMoveAction
{
    private static readonly float PERCENT_MAX_HEALTH_RECOIL = 1/8f;

    private TerraAttack terraAttack;

    public SubstituteAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (directAttackLog.GetAttackerPosition().AddVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Substituted"), battleSystem)) {
            TerraBattlePosition terraBattlePosition = terraAttack.GetAttackerPosition();
            int recoilDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOIL);
            Debug.Log(BattleDialog.RecoilDamageMsg(terraBattlePosition.GetTerra(), recoilDamage));
            battleSystem.UpdateTerraHP(terraBattlePosition, recoilDamage);
        }
            
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}