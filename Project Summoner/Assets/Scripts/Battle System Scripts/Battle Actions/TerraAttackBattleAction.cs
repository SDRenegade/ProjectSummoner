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

    public override void ProcessBattleAction(BattleSystem battleSystem, BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraAttackList().Add(terraAttack);
        terraAttack.GetTerraMoveBase()?.AddBattleActions(battleSystem);
    }
}
