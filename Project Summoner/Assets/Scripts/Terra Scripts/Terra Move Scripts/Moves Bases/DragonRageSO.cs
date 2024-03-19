using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dragon Rage")]
public class DragonRageSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DragonRage(terraAttack, this);
    }
}

public class DragonRage : TerraMoveBase
{
    private static readonly int ATTACK_DAMAGE = 10;

    public DragonRage(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += SetDragonRageDamage;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
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
