using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Absorb")]
public class AbsorbSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Absorb(terraAttack, this);
    }
}

public class Absorb : TerraMoveBase
{
    public Absorb(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        if (directAttackLog.GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)directAttackLog.GetDamage() / 2f);
            battleSystem.HealTerra(attackerPosition, healthAbsorbed);
        }
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}