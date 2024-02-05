using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Skull Bash")]
public class SkullBashSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new SkullBash(terraAttack, this);
    }
}

public class SkullBash : TerraMoveBase
{
    private static readonly int DEF_MODIFICATION = 1;

    public SkullBash(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackCharging += ChargeAction;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackCharging -= ChargeAction;
    }

    private void ChargeAction(object sender, AttackChargingEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetMove() != terraAttack.GetMove())
            return;

        TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
        eventArgs.GetBattleSystem().ChanageTerraStat(attackerPosition, Stats.DEF, DEF_MODIFICATION);
    }
}