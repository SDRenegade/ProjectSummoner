using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAction
{
    protected TerraBattlePosition terraBattlePosition;

    protected BattleAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
    }

    public abstract void ProcessBattleAction(BattleSystem battleSystem, BattleActionManager battleActionManager);
    
    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }
}
