using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraFaintEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;

    public TerraFaintEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }
}
