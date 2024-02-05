using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Explosion")]
public class ExplosionSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Explosion(terraAttack, this);
    }
}

//TODO Add an AttackMissed method for taking damage even when the attack misses
public class Explosion : TerraMoveBase
{
    public Explosion(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition terraBattlePosition = directAttackLog.GetAttackerPosition();
        battleSystem.DamageTerra(terraBattlePosition, terraBattlePosition.GetTerra().GetCurrentHP());
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}