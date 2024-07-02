using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerraSwitch
{
    private TerraBattlePosition battlePosition;
    private int benchPositionIndex;

    public TerraSwitch(TerraBattlePosition battlePosition, int benchPositionIndex)
    {
        this.battlePosition = battlePosition;
        this.benchPositionIndex = benchPositionIndex;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return battlePosition; }

    public int GetBenchPositionIndex() {  return benchPositionIndex; }
}
