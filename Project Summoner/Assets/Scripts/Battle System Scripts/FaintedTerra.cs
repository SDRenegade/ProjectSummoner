using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaintedTerra
{
    private TerraBattlePosition terraBattlePosition;
    private int faintedTerraPartyIndex;
    private bool isPrimarySide;

    public FaintedTerra(TerraBattlePosition terraBattlePosition, int faintedTerraPartyIndex, bool isPrimarySide)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.faintedTerraPartyIndex = faintedTerraPartyIndex;
        this.isPrimarySide = isPrimarySide;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public int GetFaintedTerraPartyIndex() {  return faintedTerraPartyIndex; }

    public bool IsPrimarySide() {  return isPrimarySide; }
}
