using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sucker Punch")]
public class SuckerPunchBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SuckerPunchAction(terraAttack);
    }
}

public class SuckerPunchAction : TerraMoveAction
{
    private TerraAttack terraAttack;

    public SuckerPunchAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState += DetermineAttackResult;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState -= DetermineAttackResult;
    }

    private void DetermineAttackResult(object sender, BattleEventArgs eventArgs)
    {
        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        bool isOpponenetAttacking = false;
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == this.terraAttack.GetDefendersPositionList()[0]) {
                if (terraAttack.GetMove().GetMoveBase().GetDamageType() != DamageType.STATUS)
                    isOpponenetAttacking = true;
                break;
            }
        }

        terraAttack.SetCanceled(!isOpponenetAttacking);
    }
}
