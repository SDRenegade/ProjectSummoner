using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraFaintedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;

    public TerraFaintedEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }
}
