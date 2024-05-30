using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerraSwitch
{
    private TerraBattlePosition battlePosition;
    private int benchPositionIndex;
    private bool isPrimarySide;

    public TerraSwitch(TerraBattlePosition battlePosition, int benchPositionIndex, bool isPrimarySide)
    {
        this.battlePosition = battlePosition;
        this.benchPositionIndex = benchPositionIndex;
        this.isPrimarySide = isPrimarySide;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return battlePosition; }

    public int GetBenchPositionIndex() {  return benchPositionIndex; }

    public bool IsPrimarySide() {  return isPrimarySide; }
}
