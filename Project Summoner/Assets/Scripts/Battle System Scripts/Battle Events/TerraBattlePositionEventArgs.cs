using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraBattlePositionEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra terra;

    public TerraBattlePositionEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        terra = terraBattlePosition.GetTerra();
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GetTerra() { return terra; }
}
