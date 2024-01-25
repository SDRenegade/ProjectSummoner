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
        TerraBattlePosition terraBattlePosition = terraAttack.GetAttackerPosition();
        if (!terraBattlePosition.ContainsVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Substituted"))) {
            int recoilDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOIL);
            Debug.Log(BattleDialog.RecoilDamageMsg(terraBattlePosition.GetTerra(), recoilDamage));
            battleSystem.DamageTerra(terraBattlePosition, recoilDamage);
            battleSystem.AddVolatileStatusEffect(directAttackLog.GetAttackerPosition(), SODatabase.GetInstance().GetVolatileStatusEffectByName("Substituted"));
        }
        else
            Debug.Log(BattleDialog.ATTACK_FAILED);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}