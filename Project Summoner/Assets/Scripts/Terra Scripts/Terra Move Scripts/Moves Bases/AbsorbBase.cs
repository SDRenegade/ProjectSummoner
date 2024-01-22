using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Absorb")]
public class AbsorbBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new AbsorbAction();
    }
}

public class AbsorbAction : TerraMoveAction
{
    public AbsorbAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        if (directAttackLog.GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)directAttackLog.GetDamage() / 2f);
            battleSystem.HealTerra(attackerPosition, healthAbsorbed);
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}