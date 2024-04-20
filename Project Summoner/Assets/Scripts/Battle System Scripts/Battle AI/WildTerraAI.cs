using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildTerraAI : BattleAI
{

    public WildTerraAI() {}

    public void PerformAction(TerraBattlePosition terraBattlePosition, List<int> disabledMoveIndicies, BattleSystem battleSystem)
    {
        //Create a list of available moves indicies after acounting for null move slots and disabled moves
        List<int> availableMoveIndicies = new List<int>() { 0, 1, 2, 3 };
        foreach (int index in disabledMoveIndicies)
            availableMoveIndicies.Remove(index);
        for (int i = availableMoveIndicies.Count - 1; i >= 0; i--) {
            if (availableMoveIndicies[i] >= terraBattlePosition.GetTerra().GetMoves().Count || terraBattlePosition.GetTerra().GetMoves()[availableMoveIndicies[i]].GetCurrentPP() <= 0)
                availableMoveIndicies.RemoveAt(i);
        }

        bool isPrimarySide = terraBattlePosition.GetBattleSide().IsPrimarySide();
        TerraBattlePosition[] opponentTerraBattlePositionArr = isPrimarySide ? battleSystem.GetBattlefield().GetSecondaryBattleSide().GetTerraBattlePositionArr() : battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
        int targetPositionIndex = Random.Range(0, opponentTerraBattlePositionArr.Length);
        TerraBattlePosition targetPosition = opponentTerraBattlePositionArr[targetPositionIndex];
        
        //Choose a valid move index at random. If there are no valid move indicies, use struggle.
        if(availableMoveIndicies.Count == 0) {
            TerraMove struggle = new TerraMove(SODatabase.GetInstance().GetTerraMoveByName("Struggle"));
            battleSystem.GetBattleActionManager().GetTerraAttackList().Add(
                new TerraAttack(terraBattlePosition, targetPosition, struggle));
        }
        else {
            int selectedMoveIndex = availableMoveIndicies[Random.Range(0, availableMoveIndicies.Count)];
            TerraMove selectedMove = terraBattlePosition.GetTerra().GetMoves()[selectedMoveIndex];
            battleSystem.GetBattleActionManager().GetTerraAttackList().Add(
                new TerraAttack(terraBattlePosition, targetPosition, selectedMove));
        }
    }

    public int? SwitchFaintedTerra(FaintedTerra faintedTerra, BattleSystem battleSystem)
    {
        // Temp Since wild terra encounters should never have bench terra in the terra list.
        bool isPirmarySide = faintedTerra.IsPrimarySide();
        List<Terra> terraList = isPirmarySide ? battleSystem.GetPrimaryTerraList() : battleSystem.GetSecondaryTerraList();
        for(int i = battleSystem.GetBattleFormat().NumberOfLeadingPositions(); i < terraList.Count; i++) {
            if (terraList[i].GetCurrentHP() > 0)
                return i;
        }

        return null;
    }
}
