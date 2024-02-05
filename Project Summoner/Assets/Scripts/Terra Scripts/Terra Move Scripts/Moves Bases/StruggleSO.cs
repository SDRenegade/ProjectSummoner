using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Struggle")]
public class StruggleSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Struggle(terraAttack, this);
    }
}

public class Struggle : TerraMoveBase
{
    public static readonly float PERCENT_MAX_HEALTH_RECOIL = 1/10f;

    public Struggle(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition terraBattlePosition = directAttackLog.GetAttackerPosition();

        int recoilDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOIL);
        Debug.Log(BattleDialog.RecoilDamageMsg(terraBattlePosition.GetTerra(), recoilDamage));
        battleSystem.DamageTerra(terraBattlePosition, recoilDamage);
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}