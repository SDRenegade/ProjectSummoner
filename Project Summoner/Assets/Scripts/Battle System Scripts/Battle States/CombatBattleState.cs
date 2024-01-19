using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();
        
        OpponentBattleAI(battleSystem);

        PlayerEscapeCheck(battleSystem);
        TerraSwitchCheck(battleSystem);
        ItemActiveCheck(battleSystem);
        BerryActiveCheck(battleSystem);

        //Sorts the TerraAttacks in the queued list by move priority and then by Terra speed
        List<TerraAttack> queuedTerraAtackList = battleSystem.GetBattlefield().GetTerraAttackList();
        for(int i = 0; i < queuedTerraAtackList.Count - 1; i++) {
            int highestPriorityAttackIndex = i;
            for(int j = i + 1; j < queuedTerraAtackList.Count; j++) {
                if (queuedTerraAtackList[highestPriorityAttackIndex].GetMove().GetMoveBase().GetMovePriority() < queuedTerraAtackList[j].GetMove().GetMoveBase().GetMovePriority()
                    || queuedTerraAtackList[highestPriorityAttackIndex].GetMove().GetMoveBase().GetMovePriority() == queuedTerraAtackList[j].GetMove().GetMoveBase().GetMovePriority()
                    && queuedTerraAtackList[highestPriorityAttackIndex].GetAttackingTerraPosition().GetTerra().GetSpeed() < queuedTerraAtackList[j].GetAttackingTerraPosition().GetTerra().GetSpeed())
                    highestPriorityAttackIndex = j;
            }

            if(highestPriorityAttackIndex != i) {
                TerraAttack tempTerraAttack = queuedTerraAtackList[i];
                queuedTerraAtackList[i] = queuedTerraAtackList[highestPriorityAttackIndex];
                queuedTerraAtackList[highestPriorityAttackIndex] = tempTerraAttack;
            }
        }

        //Loops through and executes all the attacks in the sorted list
        for (int i = 0; i < queuedTerraAtackList.Count; i++) {
            battleSystem.InvokeOnTerraAttackTerra(queuedTerraAtackList[i]);

            Debug.Log(BattleDialog.AttackUsedMsg(
                queuedTerraAtackList[i].GetAttackingTerraPosition().GetTerra(),
                queuedTerraAtackList[i].GetMove().GetMoveBase().GetMoveName()));

            foreach (TerraBattlePosition targetTerraPosition in queuedTerraAtackList[i].GetTargetTerraPositionList()) {
                if (queuedTerraAtackList[i].IsCancelled())
                    break;

                if (!CombatCalculator.HitCheck(queuedTerraAtackList[i].GetAttackingTerraPosition(), targetTerraPosition, queuedTerraAtackList[i].GetMove().GetMoveBase())) {
                    Debug.Log(BattleDialog.ATTACK_MISSED);
                    continue;
                }

                if (queuedTerraAtackList[i].GetMove().GetMoveBase().GetDamageType() != DamageType.STATUS) {
                    int attackDamage = CombatCalculator.CalculateDamage(queuedTerraAtackList[i].GetAttackingTerraPosition(), targetTerraPosition, queuedTerraAtackList[i].GetMove().GetMoveBase());
                    targetTerraPosition.GetTerra().TakeDamage(attackDamage);
                    Debug.Log(BattleDialog.DamageDealtMsg(
                        queuedTerraAtackList[i].GetAttackingTerraPosition().GetTerra(),
                        targetTerraPosition.GetTerra(),
                        attackDamage));
                }
                queuedTerraAtackList[i].GetMove().UseMove(queuedTerraAtackList[i].GetAttackingTerraPosition(), targetTerraPosition, battleSystem);

                battleSystem.UpdateTerraStatusBars();

                //Checks if the defending terra has fainted. If so, the game ends (temp)
                if(targetTerraPosition.GetTerra().GetCurrentHP() <= 0) {
                    Debug.Log(BattleDialog.TerraFaintedMsg(targetTerraPosition.GetTerra()));
                    battleSystem.EndBattle();
                }
            }
        }
        queuedTerraAtackList.Clear();

        battleManager.SwitchState(battleManager.GetEndTurnState());
    }

    private void OpponentBattleAI(BattleSystem battleSystem)
    {
        //Temp, just used until enemy AI is made
        if (battleSystem.GetBattleType() == BattleType.WILD) {
            TerraBattlePosition opponentTerraPosition = battleSystem.GetBattlefield().GetSecondaryBattleSide().GetTerraBattlePosition();

            battleSystem.GetBattlefield().GetTerraAttackList().Add(
                new TerraAttack(
                    opponentTerraPosition,
                    battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePosition(),
                    opponentTerraPosition.GetTerra().GetMoves()[UnityEngine.Random.Range(0, opponentTerraPosition.GetTerra().GetMoves().Count)]));
        }
    }
    
    private void PlayerEscapeCheck(BattleSystem battleSystem)
    {
        if (battleSystem.GetBattleType() == BattleType.WILD && battleSystem.GetBattlefield().GetIsAttemptEscape() == true) {
            Debug.Log(BattleDialog.ATTEMPT_ESCAPE_FAIL);
            battleSystem.GetBattlefield().SetAttemptEscape(false);
            //TODO Add logic for determining if you were able to escape and then exit battle
        }
    }

    private void TerraSwitchCheck(BattleSystem battleSystem) {}

    private void ItemActiveCheck(BattleSystem battleSystem) {}

    private void BerryActiveCheck(BattleSystem battleSystem) {}

}
