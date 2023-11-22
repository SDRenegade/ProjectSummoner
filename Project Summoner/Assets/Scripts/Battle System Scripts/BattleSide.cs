using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSide
{
    private TerraBattlePosition terraBattlePosition;
    //private List<BattleHazard> hazardList;

    public BattleSide(Terra leadingTerra)
    {
        terraBattlePosition = new TerraBattlePosition(leadingTerra, this);
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public void SetTerraBattlePosition(TerraBattlePosition terraBattlePosition) { this.terraBattlePosition = terraBattlePosition; }
}
