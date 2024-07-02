using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Explosion")]
public class ExplosionSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Explosion(terraAttack, this);
    }
}

public class Explosion : TerraMoveBase
{
    public Explosion(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition terraBattlePosition = directAttackLog.GetAttackerPosition();
        battleSystem.RecoilDamage(terraBattlePosition, terraBattlePosition.GetTerra().GetCurrentHP());
    }

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed += ExplosionMissed;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnAttackMissed -= ExplosionMissed;
    }

    private void ExplosionMissed(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove() != terraAttack.GetMove())
            return;

        TerraBattlePosition terraBattlePosition = eventArgs.GetDirectAttackLog().GetAttackerPosition();
        eventArgs.GetBattleSystem().RecoilDamage(terraBattlePosition, terraBattlePosition.GetTerra().GetCurrentHP());
    }
}