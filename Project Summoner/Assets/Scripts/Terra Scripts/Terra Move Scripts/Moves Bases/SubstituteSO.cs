using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Substitute")]
public class SubstituteSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Substitute(terraAttack, this);
    }
}

public class Substitute : TerraMoveBase
{
    private static readonly float PERCENT_MAX_HEALTH_RECOIL = 1/8f;

    public Substitute(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
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

    public override void AddMoveListeners(BattleSystem battleSystem) {}

    public override void RemoveMoveListeners(BattleSystem battleSystem) {}
}