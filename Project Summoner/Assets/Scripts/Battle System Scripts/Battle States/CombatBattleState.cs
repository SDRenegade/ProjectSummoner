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
                if (queuedTerraAttackList[highestPriorityAttackIndex].GetMovePriority() > queuedTerraAttackList[j].GetMovePriority())
                    continue;
                else if(queuedTerraAttackList[highestPriorityAttackIndex].GetMovePriority() == queuedTerraAttackList[j].GetMovePriority()) {
                    if (queuedTerraAttackList[highestPriorityAttackIndex].GetSpeedPiority() > queuedTerraAttackList[j].GetSpeedPiority())
                        continue;
                    else if(queuedTerraAttackList[highestPriorityAttackIndex].GetSpeedPiority() == queuedTerraAttackList[j].GetSpeedPiority()
                        && queuedTerraAttackList[highestPriorityAttackIndex].GetAttackerPosition().GetTerra().GetSpeed() >= queuedTerraAttackList[j].GetAttackerPosition().GetTerra().GetSpeed())
                        continue;
                }

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

        //If the attack is canceled, remove battle actions on the canceled attack and
        //continue to the next attack
        if (terraAttack.IsCanceled()) {
            terraAttack.GetTerraMoveBase()?.RemoveBattleActions(battleSystem);
            return;
        }

        if (terraAttack.IsCharging()) {
            //*** Attack Charging Event ***
            AttackChargingEventArgs attackChargingEventArgs = battleSystem.InvokeOnAttackCharging(terraAttack);

            if(attackChargingEventArgs.IsCanceled())
                terraAttack.SetCharging(false);
            else {
                Debug.Log(BattleDialog.AttackCharging(terraAttack.GetMove().GetMoveSO()));
                return;
            }
        }

        if (terraAttack.IsRecharging()) {
            Debug.Log(BattleDialog.AttackRecharging(terraAttack.GetAttackerPosition().GetTerra()));
            terraAttack.SetRecharging(false);
            return;
        }

        //Decrement the PP of the move being used
        terraAttack.GetMove().SetCurrentPP(terraAttack.GetMove().GetCurrentPP() - 1);

        //Iterate over all defending positions that the current attack is targeting
        List<DirectAttackLog> directAttackLogList = new List<DirectAttackLog>();
        for (int i = 0; i < terraAttack.GetDefendersPositionList().Count; i++) {
            TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
            TerraBattlePosition defenderPosition = terraAttack.GetDefendersPositionList()[i];
            if (defenderPosition.GetTerra() == null)
                continue;

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

                terraAttack.GetTerraMoveBase().RemoveBattleActions(battleSystem);
                continue;
            }

            directAttackLogList[i].SetSuccessfulHit(true);

            for (int j = 0; j < directAttackLogList[i].GetDirectAttackParams().GetHitCount(); j++)
                DamageStep(terraAttack, directAttackLogList[i], battleSystem);

            terraAttack.GetTerraMoveBase()?.PostAttackEffect(directAttackLogList[i], battleSystem);

            //*** Post Attack Event ***
            battleSystem.InvokeOnPostAttack(directAttackLogList[i]);

            //Checks if there is a recharge turn to the terra move that was used
            if (terraAttack.GetMove().GetMoveSO().HasRechargeTurn()) {
                //*** Attack Recharging Event ***
                AttackChargingEventArgs attackRechargingEventArgs = battleSystem.InvokeOnAttackRecharging(terraAttack);

                if (!attackRechargingEventArgs.IsCanceled())
                    terraAttack.SetRecharging(true);
            }

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
        if (terraAttack.GetMove().GetMoveSO().GetDamageType() == DamageType.STATUS || directAttackLog.GetDirectAttackParams().IsDamageStepCanceled())
            return;

        directAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(directAttackLog.GetDirectAttackParams()));
        directAttackLog.SetDamage(CombatCalculator.CalculateDamage(directAttackLog.GetDirectAttackParams(), directAttackLog.IsCrit()));

        //*** Terra Damage by Terra Event ***
        TerraDamageByTerraEventArgs terraDamageByTerraEventArgs = battleSystem.InvokeOnTerraDamageByTerra(terraAttack, directAttackLog);

        if (directAttackLog.IsCrit())
            Debug.Log(BattleDialog.CRITICAL_HIT);
        battleSystem.ApplyTerraDamage(directAttackLog.GetDefenderPosition(), directAttackLog.GetDamage());

        //*** Post Terra Damaged by Terra Event ***
        battleSystem.InvokeOnPostTerraDamageByTerra(terraDamageByTerraEventArgs);
    }
}
