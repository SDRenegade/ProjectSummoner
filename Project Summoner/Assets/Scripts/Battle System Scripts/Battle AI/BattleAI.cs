using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAI
{
    //Might be able to use the EnteringActionSelectionEventArgs as an argument instead of both the battle
    //position and disabled move indicies
    void PerformAction(TerraBattlePosition terraBattlePosition, List<int> disabledMoveIndicies, BattleSystem battleSystem);

    int? SwitchFaintedTerra(FaintedTerra faintedTerra);
}
