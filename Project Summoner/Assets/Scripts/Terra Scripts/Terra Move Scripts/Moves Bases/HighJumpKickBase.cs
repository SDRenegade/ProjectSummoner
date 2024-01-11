using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/High Jump Kick")]
public class HighJumpKickBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new HighJumpKickAction(terraAttack);
    }
}

public class HighJumpKickAction : TerraMoveAction
{
    private TerraAttack terraAttack;

    public HighJumpKickAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed += MissedAttackRecoil;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed -= MissedAttackRecoil;
    }

    private void MissedAttackRecoil(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove())
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        Terra terra = eventArgs.GetDirectAttackLog().GetAttackerPosition().GetTerra();
        if (!eventArgs.GetDirectAttackLog().IsSuccessfulHit()) {
            int recoilDamage = (int)eventArgs.GetDirectAttackLog().GetDamage() * 2;
            terra.TakeDamage(recoilDamage);
            Debug.Log(BattleDialog.HighJumpKickMissedMsg(terra, recoilDamage));
        }

        eventArgs.GetBattleSystem().OnAttackMissed -= MissedAttackRecoil;
    }
}