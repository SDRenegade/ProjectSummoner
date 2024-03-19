using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttackBattleAction : BattleAction
{
    private TerraAttack terraAttack;

    public TerraAttackBattleAction(TerraBattlePosition terraBattlePosition, TerraAttack terraAttack) : base(terraBattlePosition)
    {
        this.terraAttack = terraAttack;
    }

    public override void Execute(BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraAttackList().Add(terraAttack);
    }

    public override void Undo(BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraAttackList().Remove(terraAttack);
    }
}
