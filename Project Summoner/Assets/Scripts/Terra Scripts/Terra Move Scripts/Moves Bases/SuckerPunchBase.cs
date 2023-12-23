using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sucker Punch")]
public class SuckerPunchBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        new SuckerPunchAction(terraAttack, battleSystem);
    }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackTerraList, BattleSystem battleSystem) {}
}

public class SuckerPunchAction
{
    private TerraAttack thisTerraAttack;

    public SuckerPunchAction(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        thisTerraAttack = terraAttack;
        battleSystem.OnStartingCombat += DetermineAttackResult;
    }

    private void DetermineAttackResult(object sender, BattleEventArgs eventArgs)
    {
        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        bool isOpponenetAttacking = false;
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == thisTerraAttack.GetDefendersPositionList()[0]) {
                if (terraAttack.GetMove().GetMoveBase().GetDamageType() != DamageType.STATUS)
                    isOpponenetAttacking = true;
                break;
            }
        }

        thisTerraAttack.SetIsCanceled(!isOpponenetAttacking);
    }
}
