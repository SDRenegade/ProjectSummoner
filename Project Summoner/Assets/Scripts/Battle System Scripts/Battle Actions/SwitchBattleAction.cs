using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBattleAction : BattleAction
{
    private TerraSwitch terraSwitch;

    public SwitchBattleAction(TerraBattlePosition terraBattlePosition, TerraSwitch terraSwitch) : base(terraBattlePosition)
    {
        this.terraSwitch = terraSwitch;
    }

    public override void Execute(BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraSwitchList().Add(terraSwitch);
    }

    public override void Undo(BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraSwitchList().Remove(terraSwitch);
    }
}
