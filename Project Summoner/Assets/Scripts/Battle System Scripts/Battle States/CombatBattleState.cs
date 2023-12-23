using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();
        
        OpponentBattleAI(battleSystem); //Might want to move to its own separate State

        //*** Entering Combat State Event ***
        battleSystem.InvokeOnEnteringCombatState();

        PlayerEscapeCheck(battleSystem);
        //TerraSwitchCheck(battleSystem);

        //*** Starting Combat Event ***
        battleSystem.InvokeOnStartingCombat();

        List<TerraAttack> queuedTerraAttackList = battleSystem.GetBattleActionManager().GetTerraAttackList();
        SortTerraAttackList(queuedTerraAttackList);

        //Loops through and executes all the attacks in the sorted list
        for (int i = 0; i < queuedTerraAttackList.Count; i++)
            ProcessTerraAttack(queuedTerraAttackList[i], battleSystem);

        queuedTerraAttackList.Clear();

        battleManager.SwitchState(battleManager.GetEndTurnState());
    }

    private void OpponentBattleAI(BattleSystem battleSystem)
    {
        //Temp, just used until enemy AI is made
        if (battleSystem.GetBattleType() == BattleType.WILD) {
            TerraBattlePosition opponentTerraPosition = battleSystem.GetBattlefield().GetSecondaryBattleSide().GetTerraBattlePosition();

            battleSystem.GetBattleActionManager().GetTerraAttackList().Add(
                new TerraAttack(
                    opponentTerraPosition,
                    battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePosition(),
                    opponentTerraPosition.GetTerra().GetMoves()[UnityEngine.Random.Range(0, opponentTerraPosition.GetTerra().GetMoves().Count)]));
        }
    }
    
    private void PlayerEscapeCheck(BattleSystem battleSystem)
    {
        if (battleSystem.GetBattleType() == BattleType.WILD && battleSystem.GetBattleActionManager().GetIsAttemptEscape() == true) {
            Debug.Log(BattleDialog.ATTEMPT_ESCAPE_FAIL);
            battleSystem.GetBattleActionManager().SetAttemptEscape(false);
            //TODO Add logic for determining if you were able to escape and then exit battle
        }
    }

    private void SortTerraAttackList(List<TerraAttack> queuedTerraAttackList)
    {
        //Sorts the TerraAttacks in the queued list by move priority and then by Terra speed
        for (int i = 0; i < queuedTerraAttackList.Count - 1; i++) {
            int highestPriorityAttackIndex = i;
            for (int j = i + 1; j < queuedTerraAttackList.Count; j++) {
                if (queuedTerraAttackList[highestPriorityAttackIndex].GetMovePriority() < queuedTerraAttackList[j].GetMovePriority()
                    || queuedTerraAttackList[highestPriorityAttackIndex].GetMovePriority() == queuedTerraAttackList[j].GetMovePriority()
                    && queuedTerraAttackList[highestPriorityAttackIndex].GetAttackerPosition().GetTerra().GetSpeed() < queuedTerraAttackList[j].GetAttackerPosition().GetTerra().GetSpeed())
                    highestPriorityAttackIndex = j;
            }

            if (highestPriorityAttackIndex != i) {
                TerraAttack tempTerraAttack = queuedTerraAttackList[i];
                queuedTerraAttackList[i] = queuedTerraAttackList[highestPriorityAttackIndex];
                queuedTerraAttackList[highestPriorityAttackIndex] = tempTerraAttack;
            }
        }
    }

    private void ProcessTerraAttack(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        Debug.Log(BattleDialog.AttackUsedMsg(terraAttack));

        //*** Terra Attack Declaration Event *** / Might not need this event
        battleSystem.InvokeOnTerraAttackDeclaration(terraAttack);

        //If the attack is flinched, continue to the next attack
        if (terraAttack.IsFlinched()) {
            Debug.Log(BattleDialog.FlinchedMsg(terraAttack.GetAttackerPosition().GetTerra()));
            return;
        }
        //If the attack is canceled, continue to the next attack
        if (terraAttack.IsCanceled()) {
            Debug.Log(BattleDialog.ATTACK_FAILED);
            return;
        }

        //Iterate over all defending positions that the current attack is targeting
        List<TerraAttackLog> terraAttackLogList = new List<TerraAttackLog>();
        for (int i = 0; i < terraAttack.GetDefendersPositionList().Count; i++) {
            TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
            TerraBattlePosition defenderPosition = terraAttack.GetDefendersPositionList()[i];
            TerraAttackParams terraAttackParams = new TerraAttackParams(attackerPosition, defenderPosition, terraAttack.GetMove());

            //Add this attack as a new log in the list
            terraAttackLogList.Add(new TerraAttackLog(attackerPosition, defenderPosition));

            //*** Terra Direct Attack Event ***
            battleSystem.InvokeOnTerraDirectAttack(terraAttackParams);

            //Accuracy Check. If false, the move misses and we continue to the next defending battle position.
            if (!(terraAttackParams.IsMustHit() || CombatCalculator.HitCheck(terraAttackParams))) {
                Debug.Log(BattleDialog.ATTACK_MISSED);
                continue;
            }

            terraAttackLogList[i].SetSuccessfulHit(true);

            //Before damage calculation we activate the move's pre attack effect
            terraAttack.GetMove().GetMoveBase().PreAttackEffect(terraAttackParams, battleSystem);

            //If the move used is not a status move, we calculate the damage dealt
            if (terraAttack.GetMove().GetMoveBase().GetDamageType() != DamageType.STATUS) {
                for (int j = 0; j < terraAttackParams.GetHitCount(); j++)
                    ProcessDamageStep(terraAttack, terraAttackParams, terraAttackLogList[i], battleSystem);

                if (terraAttackParams.GetHitCount() > 1)
                    Debug.Log(BattleDialog.MultiHitMsg(
                        terraAttackParams.GetAttackerPosition().GetTerra(),
                        terraAttackParams.GetHitCount()));
            }

            battleSystem.UpdateTerraStatusBars();

            //Checks if the defending terra has fainted. If so, the game ends (temp)
            if (defenderPosition.GetTerra().GetCurrentHP() <= 0) {
                Debug.Log(BattleDialog.TerraFaintedMsg(defenderPosition.GetTerra()));
                battleSystem.EndBattle();
                return;
            }
        }
        //After damage calculation we activate the move's post attack effect
        terraAttack.GetMove().GetMoveBase().PostAttackEffect(terraAttackLogList, battleSystem);

        battleSystem.UpdateTerraStatusBars();

        //TODO Add check for fainted terra, since terra can take recoil damage from a
        //post attack effect
    }

    private void ProcessDamageStep(TerraAttack terraAttack, TerraAttackParams terraAttackParams, TerraAttackLog terraAttackLog, BattleSystem battleSystem)
    {
        terraAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(terraAttackParams));

        int? damage = CombatCalculator.CalculateDamage(terraAttackParams, terraAttackLog.IsCrit());

        //*** Terra Damaged by Terra Event ***
        TerraDamageByTerraEventArgs eventArgs = battleSystem.InvokeOnTerraDamageByTerra(terraAttack, terraAttackLog, damage);
        damage = eventArgs.GetDamage();

        if (damage != null) {
            terraAttackLog.GetDefenderPosition().GetTerra().TakeDamage((int)damage);

            terraAttackLog.AddDamage(damage);

            if (terraAttackLog.IsCrit())
                Debug.Log(BattleDialog.CRITICAL_HIT);

            Debug.Log(BattleDialog.DamageDealtMsg(
                terraAttackLog.GetAttackerPosition().GetTerra(),
                terraAttackLog.GetDefenderPosition().GetTerra(),
                (int)damage));
        }
    }
}
