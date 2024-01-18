using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningMoveSelectionUIEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private List<int> disabledMoveIndicies;

    public OpeningMoveSelectionUIEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        disabledMoveIndicies = new List<int>();
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public List<int> GetDisabledMoveIndicies() { return disabledMoveIndicies; }

    public void AddDisabledMoveIndex(int moveIndex)
    {
        foreach (int index in disabledMoveIndicies) {
            if (index == moveIndex)
                return;
        }

        disabledMoveIndicies.Add(moveIndex);
    }
}
