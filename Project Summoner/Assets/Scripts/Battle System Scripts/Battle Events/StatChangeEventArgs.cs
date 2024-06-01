using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private Stats stat;
    private StatStages initialStatStage;
    private int modification;
    private bool isCanceled;

    public StatChangeEventArgs(TerraBattlePosition terraBattlePosition, Stats stat, int modification, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.stat = stat;
        initialStatStage = terraBattlePosition.GetStatStage(stat);
        this.modification = modification;
        isCanceled = false;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public Stats GetStat() { return stat; }

    public StatStages GetInitialStatStage() {  return initialStatStage; }

    public int GetModification() { return modification; }

    public void SetModification(int modification) {  this.modification = modification; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
