using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Take Down")]
public class TakeDownBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new TakeDownAction();
    }
}

public class TakeDownAction : TerraMoveAction
{
    public static readonly float PERCENT_MAX_HEALTH_RECOIL = 1/10f;

    public TakeDownAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition terraBattlePosition = directAttackLog.GetAttackerPosition();

        int recoilDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOIL);
        Debug.Log(BattleDialog.RecoilDamageMsg(terraBattlePosition.GetTerra(), recoilDamage));
        battleSystem.DamageTerra(terraBattlePosition, recoilDamage);
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}