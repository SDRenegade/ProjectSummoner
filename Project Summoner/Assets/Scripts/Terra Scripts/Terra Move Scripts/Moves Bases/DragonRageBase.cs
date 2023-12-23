using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dragon Rage")]
public class DragonRageBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        new DragonRageAction(terraAttack, battleSystem);
    }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem) {}
}

public class DragonRageAction
{
    readonly int ATTACK_DAMAGE = 5;

    private TerraAttack thisTerraAttack;

    public DragonRageAction(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        thisTerraAttack = terraAttack;
        battleSystem.OnTerraDamageByTerra += SetDragonRageDamage;
    }

    private void SetDragonRageDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (thisTerraAttack != eventArgs.GetTerraAttack())
            return;

        eventArgs.SetDamage(ATTACK_DAMAGE);
    }

}
