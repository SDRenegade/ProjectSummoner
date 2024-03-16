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

    public override void ProcessBattleAction(BattleSystem battleSystem, BattleActionManager battleActionManager)
    {
        battleActionManager.GetTerraSwitchList().Add(terraSwitch);
    }
}
