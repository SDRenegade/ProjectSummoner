using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Super Fang")]
public class SuperFangBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        new SuperFangAction(terraAttack, battleSystem);
    }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}
    
    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem) {}
}

public class SuperFangAction
{
    private readonly float PERCENT_HEALTH_DAMAGE = 1/2f;

    private TerraAttack thisTerraAttack;

    public SuperFangAction(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        thisTerraAttack = terraAttack;
        battleSystem.OnTerraDamageByTerra += SetSuperFangDamage;
    }

    private void SetSuperFangDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (thisTerraAttack != eventArgs.GetTerraAttack())
            return;

        Terra defendingTerra = eventArgs.GetTerraAttacLog().GetDefenderPosition().GetTerra();
        eventArgs.SetDamage((int)(defendingTerra.GetCurrentHP() * PERCENT_HEALTH_DAMAGE));
    }
}
