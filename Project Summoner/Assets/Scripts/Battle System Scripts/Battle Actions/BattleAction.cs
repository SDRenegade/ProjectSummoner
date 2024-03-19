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

    public abstract void Execute(BattleActionManager battleActionManager);

    public abstract void Undo(BattleActionManager battleActionManager);
    
    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }
}
