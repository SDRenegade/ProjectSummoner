using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringActionSelectionEventArgs : BattleEventArgs
{
    private TerraBattlePosition terraBattlePosition;
    private List<int> disabledMoveIndicies;
    private bool isSkipActionSelection;

    public EnteringActionSelectionEventArgs(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) : base(battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        disabledMoveIndicies = new List<int>();
        isSkipActionSelection = false;
    }

    public TerraBattlePosition GetTerraBattlePosition() { return terraBattlePosition; }

    public bool IsSkipActionSelection() { return isSkipActionSelection; }

    public void SetSkipActionSelection(bool isSkipActionSelection) { this.isSkipActionSelection = isSkipActionSelection; }

    public List<int> GetDisabledMoveIndicies() { return disabledMoveIndicies; }

    public void AddDisabledMoveIndex(int moveIndex)
    {
        foreach(int index in disabledMoveIndicies) {
            if (index == moveIndex)
                return;
        }

        disabledMoveIndicies.Add(moveIndex);
    }
}
