using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBattleState : BattleState
{
    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** Entering Combat State Event ***
        battleSystem.InvokeOnEnteringCombatState();

        //PlayerEscapeCheck(battleSystem);
        //TerraSwitchCheck(battleSystem);

        List<TerraAttack> queuedTerraAttackList = battleSystem.GetBattleActionManager().GetTerraAttackList();
        SortTerraAttackList(queuedTerraAttackList);

        //Loops through and executes all the attacks in the sorted list
        for (int i = 0; i < queuedTerraAttackList.Count; i++) {
            ProcessTerraAttack(queuedTerraAttackList[i], battleSystem);
            if (battleSystem.IsMatchFinished())
                break;
        }

        if(battleSystem.IsMatchFinished())
            battleManager.SwitchState(battleManager.GetFinishedMatchBattleState());
        else
            battleManager.SwitchState(battleManager.GetEndTurnState());
    }
    
    /*private void PlayerEscapeCheck(BattleSystem battleSystem)
    {
        if (battleSystem.GetBattleType() == BattleType.WILD && battleSystem.GetBattleActionManager().GetIsAttemptEscape() == true) {
            Debug.Log(BattleDialog.ATTEMPT_ESCAPE_FAIL);
            battleSystem.GetBattleActionManager().SetAttemptEscape(false);
            //TODO Add logic for determining if you were able to escape and then exit battle
        }
    }*/

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

        //*** Terra Attack Declaration Event ***
        battleSystem.InvokeOnAttackDeclaration(terraAttack);

        //If the attack is canceled, continue to the next attack
        if (terraAttack.IsCanceled()) {
            terraAttack.GetTerraMoveAction()?.RemoveBattleActions(battleSystem);
            return;
        }

        //Decrement the PP of the move being used
        terraAttack.GetMove().SetCurrentPP(terraAttack.GetMove().GetCurrentPP() - 1);

        //Iterate over all defending positions that the current attack is targeting
        List<DirectAttackLog> directAttackLogList = new List<DirectAttackLog>();
        for (int i = 0; i < terraAttack.GetDefendersPositionList().Count; i++) {
            TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
            TerraBattlePosition defenderPosition = terraAttack.GetDefendersPositionList()[i];

            //Add this attack as a new log in the list
            directAttackLogList.Add(new DirectAttackLog(attackerPosition, defenderPosition, terraAttack.GetMove()));

            //*** Direct Attack Event ***
            DirectAttackEventArgs directAttackEventArgs = battleSystem.InvokeOnDirectAttack(directAttackLogList[i].GetDirectAttackParams());

            if(directAttackEventArgs.IsCanceled())
                continue;

            //Accuracy Check. If false, the move misses and we continue to the next defending battle position.
            if (!CombatCalculator.HitCheck(directAttackLogList[i].GetDirectAttackParams())) {
                Debug.Log(BattleDialog.ATTACK_MISSED);
                //*** Attack Missed Event ***
                battleSystem.InvokeOnAttackMissed(directAttackLogList[i]);

                if (directAttackLogList[i].GetDirectAttackParams().GetHitCount() > 1)
                    Debug.Log(BattleDialog.MultiHitMsg(
                        directAttackLogList[i].GetDirectAttackParams().GetAttackerPosition().GetTerra(),
                        directAttackLogList[i].GetDirectAttackParams().GetHitCount()));

                continue;
            }

            directAttackLogList[i].SetSuccessfulHit(true);

            for (int j = 0; j < directAttackLogList[i].GetDirectAttackParams().GetHitCount(); j++)
                DamageStep(terraAttack, directAttackLogList[i], battleSystem);

            //After damage is calculated and applied we activate the move's post attack effect
            terraAttack.GetTerraMoveAction()?.PostAttackEffect(directAttackLogList[i], battleSystem);

            if (directAttackLogList[i].GetDirectAttackParams().GetHitCount() > 1)
                Debug.Log(BattleDialog.MultiHitMsg(
                    directAttackLogList[i].GetDirectAttackParams().GetAttackerPosition().GetTerra(),
                    directAttackLogList[i].GetDirectAttackParams().GetHitCount()));

            battleSystem.UpdateTerraStatusBars();
        }
    }

    //If the move used is not a status move and the damage step is not canceled, we calculate the damage dealt
    private void DamageStep(TerraAttack terraAttack, DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (terraAttack.GetMove().GetMoveBase().GetDamageType() == DamageType.STATUS || directAttackLog.GetDirectAttackParams().IsDamageStepCanceled())
            return;

        directAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(directAttackLog.GetDirectAttackParams()));
        directAttackLog.SetDamage(CombatCalculator.CalculateDamage(directAttackLog.GetDirectAttackParams(), directAttackLog.IsCrit()));

        //*** Terra Damage by Terra Event ***
        battleSystem.InvokeOnTerraDamageByTerra(terraAttack, directAttackLog);

        if (directAttackLog.IsCrit())
            Debug.Log(BattleDialog.CRITICAL_HIT);
        battleSystem.UpdateTerraHP(directAttackLog.GetDefenderPosition(), -directAttackLog.GetDamage());
    }
}
