using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildTerraAI : BattleAI
{
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

        int targetPositionIndex = Random.Range(0, battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr().Length);
        TerraBattlePosition targetPosition = battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr()[targetPositionIndex];
        
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
}
