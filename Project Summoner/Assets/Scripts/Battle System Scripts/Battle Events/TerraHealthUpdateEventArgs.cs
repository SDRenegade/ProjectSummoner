using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraHealthUpdateEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private int? hpUpdate;

    public TerraHealthUpdateEventArgs(TerraBattlePosition terraBattlePosition, int? hpUpdate, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.hpUpdate = hpUpdate;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public int? GetHealthUpdate() { return hpUpdate; }

    public void SetHealthUpdate(int? hpUpdate) { this.hpUpdate = hpUpdate; }
}
