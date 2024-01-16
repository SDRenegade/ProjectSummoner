using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dragon Rage")]
public class DragonRageBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DragonRageAction(terraAttack);
    }
}

public class DragonRageAction : TerraMoveAction
{
    private static readonly int ATTACK_DAMAGE = 5;

    private TerraAttack terraAttack;

    public DragonRageAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += SetDragonRageDamage;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= SetDragonRageDamage;
    }

    private void SetDragonRageDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        eventArgs.GetDirectAttackLog().SetDamage(ATTACK_DAMAGE);
    }
}
