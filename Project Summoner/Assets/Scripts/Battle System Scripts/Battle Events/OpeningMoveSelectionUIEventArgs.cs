using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningMoveSelectionUIEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private List<int> disabledMoveIndicies;
    private bool isMoveSelectionCanceled;

    public OpeningMoveSelectionUIEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        disabledMoveIndicies = new List<int>();
        isMoveSelectionCanceled = false;
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

    public bool IsMoveSelectionCanceled() { return isMoveSelectionCanceled; }

    public void SetMoveSelectionCancled(bool isMoveSelectionCanceled) { this.isMoveSelectionCanceled = isMoveSelectionCanceled; }
}
