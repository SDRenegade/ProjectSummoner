using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Super Fang")]
public class SuperFangSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SuperFang(terraAttack, this);
    }
}

public class SuperFang : TerraMoveBase
{
    private readonly float PERCENT_HEALTH_DAMAGE = 1/2f;

    public SuperFang(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += SetPercentMaxHealthDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= SetPercentMaxHealthDamage;
    }

    private void SetPercentMaxHealthDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        Terra defendingTerra = eventArgs.GetDirectAttackLog().GetDefenderPosition().GetTerra();
        eventArgs.GetDirectAttackLog().SetDamage((int)(defendingTerra.GetCurrentHP() * PERCENT_HEALTH_DAMAGE));
    }
}
