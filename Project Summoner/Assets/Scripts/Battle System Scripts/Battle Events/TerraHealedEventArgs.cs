using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraHealedEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Terra healedTerra;
    private int? healAmt;

    public TerraHealedEventArgs(TerraBattlePosition terraBattlePosition, int? healAmt, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        healedTerra = terraBattlePosition.GetTerra();
        this.healAmt = healAmt;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Terra GethealedTerra() {  return healedTerra; }

    public int? GetHealAmt() { return healAmt; }

    public void SetHealAmt(int? healAmt) { this.healAmt = healAmt; }
}
