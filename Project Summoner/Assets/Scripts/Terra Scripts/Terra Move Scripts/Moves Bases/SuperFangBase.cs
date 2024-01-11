using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Super Fang")]
public class SuperFangBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SuperFangAction(terraAttack);
    }
}

public class SuperFangAction : TerraMoveAction
{
    private readonly float PERCENT_HEALTH_DAMAGE = 1/2f;

    private TerraAttack terraAttack;

    public SuperFangAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += SetSuperFangDamage;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= SetSuperFangDamage;
    }

    private void SetSuperFangDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        Terra defendingTerra = eventArgs.GetDirectAttackLog().GetDefenderPosition().GetTerra();
        eventArgs.SetDamage((int)(defendingTerra.GetCurrentHP() * PERCENT_HEALTH_DAMAGE));
    }
}
