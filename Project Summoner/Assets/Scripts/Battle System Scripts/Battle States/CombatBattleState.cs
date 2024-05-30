using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBattleState : BattleState
{
    private BattleStateManager battleManager;
    private EscapeAttempt escapeAttempt;
    private CaptureAttempt captureAttempt;
    private Queue<TerraSwitch> terraSwitchQueue;
    private Queue<TerraAttack> terraAttackQueue;

    public CombatBattleState(BattleStateManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        escapeAttempt = battleSystem.GetBattleActionManager().GetEscapeAttempt();
        captureAttempt = battleSystem.GetBattleActionManager().GetCaptureAttempt();
        terraSwitchQueue = SortTerraSwitchs(battleSystem.GetBattleActionManager().GetTerraSwitchList());
        terraAttackQueue = SortTerraAttacks(battleSystem.GetBattleActionManager().GetTerraAttackList());

        //*** Entering Combat State Event ***
        battleSystem.InvokeOnEnteringCombatState();

        NextCombatAction(battleSystem);
    }

    public void NextCombatAction(BattleSystem battleSystem)
    {
        if (battleSystem.IsBattleFinished()) {
            battleManager.SwitchState(battleManager.GetFinishedMatchState());
            return;
        }

        if(escapeAttempt != null) {
            ProcessEscapeAttempt(battleSystem);
            BattleSequenceManager.GetInstance().StartBattleActionSequence();
        }
        else if(captureAttempt != null) {
            ProcessCaptureAttempt(battleSystem);
            BattleSequenceManager.GetInstance().StartBattleActionSequence();
        }
        else if(terraSwitchQueue.Count > 0) {
            ProcessTerraSwitches(terraSwitchQueue.Dequeue(), battleSystem);
            BattleSequenceManager.GetInstance().StartBattleActionSequence();
        }
        else if(terraAttackQueue.Count > 0) {
            ProcessTerraAttack(terraAttackQueue.Dequeue(), battleSystem);
            BattleSequenceManager.GetInstance().StartBattleActionSequence();
        }
        else
            battleManager.SwitchState(battleManager.GetEndTurnState());
    }
    
    private void ProcessEscapeAttempt(BattleSystem battleSystem)
    {
        if (escapeAttempt == null)
            return;

        battleSystem.EscapeAttempt(escapeAttempt);
        escapeAttempt = null;
    }

    private void ProcessCaptureAttempt(BattleSystem battleSystem)
    {
        if (captureAttempt == null)
            return;

        battleSystem.CaptureAttempt(captureAttempt);
        captureAttempt = null;
    }

    private Queue<TerraSwitch> SortTerraSwitchs(List<TerraSwitch> terraSwitchList)
    {
        Queue<TerraSwitch> switchQueue = new Queue<TerraSwitch>();

        //Sorts terra switches by leading terra speed
        for (int i = 0; i < terraSwitchList.Count - 1; i++) {
            int highestPriorityIndex = i;
            for (int j = i + 1; j < terraSwitchList.Count; j++) {
                if (terraSwitchList[highestPriorityIndex].GetTerraBattlePosition().GetTerra().GetSpeed() >= terraSwitchList[j].GetTerraBattlePosition().GetTerra().GetSpeed())
                    continue;

                highestPriorityIndex = j;
            }

            if (highestPriorityIndex != i) {
                TerraSwitch tmp = terraSwitchList[i];
                terraSwitchList[i] = terraSwitchList[highestPriorityIndex];
                terraSwitchList[highestPriorityIndex] = tmp;
            }
        }

        for (int i = 0; i < terraSwitchList.Count; i++)
            switchQueue.Enqueue(terraSwitchList[i]);

        return switchQueue;
    }

    private void ProcessTerraSwitches(TerraSwitch terraSwitch, BattleSystem battleSystem)
    {
        if (terraSwitch == null)
            return;
        
        battleSystem.SwitchTerra(terraSwitch);
    }

    private Queue<TerraAttack> SortTerraAttacks(List<TerraAttack> terraAttackList)
    {
        Queue<TerraAttack> attackQueue = new Queue<TerraAttack>();

        //Sorts the TerraAttacks in the queued list by move priority and then by Terra speed
        for (int i = 0; i < terraAttackList.Count - 1; i++) {
            int highestPriorityIndex = i;
            for (int j = i + 1; j < terraAttackList.Count; j++) {
                if (terraAttackList[highestPriorityIndex].GetMovePriority() > terraAttackList[j].GetMovePriority())
                    continue;
                else if(terraAttackList[highestPriorityIndex].GetMovePriority() == terraAttackList[j].GetMovePriority()) {
                    if (terraAttackList[highestPriorityIndex].GetSpeedPiority() > terraAttackList[j].GetSpeedPiority())
                        continue;
                    else if(terraAttackList[highestPriorityIndex].GetSpeedPiority() == terraAttackList[j].GetSpeedPiority()
                        && terraAttackList[highestPriorityIndex].GetAttackerPosition().GetTerra().GetSpeed() >= terraAttackList[j].GetAttackerPosition().GetTerra().GetSpeed())
                        continue;
                }

                highestPriorityIndex = j;
            }

            if (highestPriorityIndex != i) {
                TerraAttack tempTerraAttack = terraAttackList[i];
                terraAttackList[i] = terraAttackList[highestPriorityIndex];
                terraAttackList[highestPriorityIndex] = tempTerraAttack;
            }
        }

        for(int i = 0; i < terraAttackList.Count; i++)
            attackQueue.Enqueue(terraAttackList[i]);

        return attackQueue;
    }

    public void ProcessTerraAttack(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        if (terraAttack.GetAttackerPosition().GetTerra() == null) {
            Debug.LogError("Attacker position terra is null in CombatBattleState.ProcessTerraAttack");
            return;
        }

        Debug.Log(BattleDialog.AttackUsedMsg(terraAttack));
        //*** Terra Attack Declaration Event ***
        battleSystem.InvokeOnAttackDeclaration(terraAttack);

        bool hasValidTarget = false;
        foreach(TerraBattlePosition targetPosition in terraAttack.GetDefendersPositionList()) {
            if(targetPosition.GetTerra() != null) {
                hasValidTarget = true;
                break;
            }
        }

        //If the attack is canceled or there are no valid targest, cancel attack
        if (terraAttack.IsCanceled() || !hasValidTarget) {
            terraAttack.GetTerraMoveBase()?.RemoveMoveListeners(battleSystem);
            // TODO Add attack canceled event
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
            // TODO Add recharging event
            Debug.Log(BattleDialog.AttackRecharging(terraAttack.GetAttackerPosition().GetTerra()));
            terraAttack.SetRecharging(false);
            return;
        }

        //Decrement the PP of the move being used
        terraAttack.GetMove().SetCurrentPP(terraAttack.GetMove().GetCurrentPP() - 1);

        //*** Start Terra Attack Event ***
        battleSystem.InvokeOnStartTerraAttack(terraAttack);

        //Iterate over all defending positions that the current attack is targeting
        List<DirectAttackLog> directAttackLogList = new List<DirectAttackLog>();
        for (int i = 0; i < terraAttack.GetDefendersPositionList().Count; i++) {
            TerraBattlePosition attackerPosition = terraAttack.GetAttackerPosition();
            TerraBattlePosition defenderPosition = terraAttack.GetDefendersPositionList()[i];
            directAttackLogList.Add(new DirectAttackLog(attackerPosition, defenderPosition, terraAttack.GetMove()));
            
            if (defenderPosition.GetTerra() == null) {
                Debug.Log(BattleDialog.ATTACK_FAILED);
                directAttackLogList[i].SetSuccessful(false);
                continue;
            }

            directAttackLogList[i].SetSuccessful(true);

            //*** Direct Attack Event ***
            DirectAttackEventArgs directAttackEventArgs = battleSystem.InvokeOnDirectAttack(directAttackLogList[i].GetDirectAttackParams());

            if(directAttackEventArgs.IsCanceled())
                continue;

            //Accuracy Check. If false, the move misses and we continue to the next defending battle position.
            if (!CombatCalculator.HitCheck(directAttackLogList[i].GetDirectAttackParams())) {
                Debug.Log(BattleDialog.ATTACK_MISSED);
                //*** Attack Missed Event ***
                battleSystem.InvokeOnAttackMissed(directAttackLogList[i]);

                terraAttack.GetTerraMoveBase().RemoveMoveListeners(battleSystem);
                continue;
            }

            directAttackLogList[i].SetHit(true);

            for (int j = 0; j < directAttackLogList[i].GetDirectAttackParams().GetHitCount(); j++)
                DamageStep(terraAttack, directAttackLogList[i], battleSystem);

            //*** Direct Attack Hit Event ***
            battleSystem.InvokeOnDirectAttackHit(directAttackLogList[i]);

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

            // *** Temp ***
            battleSystem.DynamicUpdateStatusBar(terraAttack.GetDefendersPositionList()[i]);
        }
    }

    //If the move used is not a status move and the damage step is not canceled, we calculate the damage dealt
    private void DamageStep(TerraAttack terraAttack, DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (terraAttack.GetMove().GetMoveSO().GetDamageType() == DamageType.STATUS || directAttackLog.GetDirectAttackParams().IsDamageStepCanceled())
            return;

        directAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(directAttackLog.GetDirectAttackParams()));
        directAttackLog.SetDamage(CombatCalculator.DamageCalculation(directAttackLog.GetDirectAttackParams(), directAttackLog.IsCrit()));

        //*** Terra Damage by Terra Event ***
        TerraDamageByTerraEventArgs terraDamageByTerraEventArgs = battleSystem.InvokeOnTerraDamageByTerra(terraAttack, directAttackLog);

        if (directAttackLog.IsCrit())
            Debug.Log(BattleDialog.CRITICAL_HIT);
        battleSystem.ApplyDamage(directAttackLog.GetDefenderPosition(), directAttackLog.GetDamage());

        //*** Post Terra Damaged by Terra Event ***
        battleSystem.InvokeOnPostTerraDamageByTerra(terraDamageByTerraEventArgs);
    }
}
