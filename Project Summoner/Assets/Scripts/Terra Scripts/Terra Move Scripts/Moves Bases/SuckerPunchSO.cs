using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Sucker Punch")]
public class SuckerPunchSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SuckerPunch(terraAttack, this);
    }
}

public class SuckerPunch : TerraMoveBase
{
    public SuckerPunch(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState += DetermineAttackResult;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState -= DetermineAttackResult;
    }

    private void DetermineAttackResult(object sender, BattleEventArgs eventArgs)
    {
        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        bool isOpponenetAttacking = false;
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == this.terraAttack.GetDefendersPositionList()[0]) {
                if (terraAttack.GetMove().GetMoveSO().GetDamageType() != DamageType.STATUS)
                    isOpponenetAttacking = true;
                break;
            }
        }

        terraAttack.SetCanceled(!isOpponenetAttacking);
    }
}
