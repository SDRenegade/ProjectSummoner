using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/High Jump Kick")]
public class HighJumpKickSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new HighJumpKick(terraAttack, this);
    }
}

public class HighJumpKick : TerraMoveBase
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    public HighJumpKick(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed += MissedAttackRecoil;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed -= MissedAttackRecoil;
    }

    private void MissedAttackRecoil(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove())
            return;

        DirectAttackLog directAttackLog = eventArgs.GetDirectAttackLog();
        directAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(directAttackLog.GetDirectAttackParams()));
        directAttackLog.SetDamage(CombatCalculator.CalculateDamage(directAttackLog.GetDirectAttackParams(), directAttackLog.IsCrit()));

        //*** Terra Damage by Terra Event ***
        eventArgs.GetBattleSystem().InvokeOnTerraDamageByTerra(terraAttack, directAttackLog);

        if(directAttackLog.GetDamage() != null)
            directAttackLog.SetDamage((int)(directAttackLog.GetDamage() * DAMAGE_MULTIPLIER));

        TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
        eventArgs.GetBattleSystem().DamageTerra(attackerPosition, directAttackLog.GetDamage());

        if (directAttackLog.IsCrit())
            Debug.Log(BattleDialog.CRITICAL_HIT);
        if (directAttackLog.GetDamage() != null)
            Debug.Log(BattleDialog.HighJumpKickMissedMsg(attackerPosition.GetTerra(), (int)directAttackLog.GetDamage()));
    }
}