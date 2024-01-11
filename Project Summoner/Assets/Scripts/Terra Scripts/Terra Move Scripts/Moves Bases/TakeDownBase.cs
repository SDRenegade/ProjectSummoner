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
    public static readonly float RECOIL_DAMAGE_PERCENT = 1/10f;

    public TakeDownAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        Terra attacker = directAttackLog.GetAttackerPosition().GetTerra();

        int recoilDamage = (int)(attacker.GetMaxHP() * RECOIL_DAMAGE_PERCENT);
        attacker.TakeDamage(recoilDamage);
        Debug.Log(BattleDialog.RecoilDamageMsg(attacker, recoilDamage));
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}