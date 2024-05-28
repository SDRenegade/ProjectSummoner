using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBattleState : BattleState
{
    private BattleStateManager battleManager;
    private Queue<TerraAttack> terraAttackQueue;

    public CombatBattleState(BattleStateManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public void EnterState(BattleStateManager battleManager)
    {
        BattleSystem battleSystem = battleManager.GetBattleSystem();

        //*** Entering Combat State Event ***
        battleSystem.InvokeOnEnteringCombatState();

        ProcessEscapeAttempt(battleSystem);
        if(battleSystem.IsBattleFinished()) {
            battleManager.SwitchState(battleManager.GetFinishedMatchState());
            return;
        }
        ProcessCaptureAttempts(battleSystem);
        if (battleSystem.IsBattleFinished()) {
            battleManager.SwitchState(battleManager.GetFinishedMatchState());
            return;
        }
        ProcessTerraSwitches(battleSystem);

        terraAttackQueue = SortTerraAttackList(battleSystem.GetBattleActionManager().GetTerraAttackList());
        NextCombatAction(battleSystem);
    }

    public void NextCombatAction(BattleSystem battleSystem)
    {
        if (battleSystem.IsBattleFinished()) {
            battleManager.SwitchState(battleManager.GetFinishedMatchState());
            return;
        }

        if (terraAttackQueue.Count > 0) {
            ProcessTerraAttack(terraAttackQueue.Dequeue(), battleSystem);
            BattleSequenceManager.GetInstance().StartBattleActionSequence();
        }
        else
            battleManager.SwitchState(battleManager.GetEndTurnState());
    }
    
    private void ProcessEscapeAttempt(BattleSystem battleSystem)
    {
        if(battleSystem.GetBattleActionManager().GetEscapeAttempt() != null)
            battleSystem.EscapeAttempt(battleSystem.GetBattleActionManager().GetEscapeAttempt());
    }

    private void ProcessTerraSwitches(BattleSystem battleSystem)
    {
        BattleActionManager battleActionManager = battleSystem.GetBattleActionManager();
        for(int i = 0; i < battleActionManager.GetTerraSwitchList().Count; i++)
            battleSystem.SwitchTerra(battleActionManager.GetTerraSwitchList()[i]);
    }

    private void ProcessCaptureAttempts(BattleSystem battleSystem)
    {
        BattleActionManager battleActionManager = battleSystem.GetBattleActionManager();
        for (int i = 0; i < battleActionManager.GetCaptureAttemptList().Count; i++) {
            battleSystem.CaptureAttempt(battleActionManager.GetCaptureAttemptList()[i]);
            if (battleSystem.IsBattleFinished())
                break;
        }
    }

    private Queue<TerraAttack> SortTerraAttackList(List<TerraAttack> terraAttackList)
    {
        Queue<TerraAttack> terraAttackQueue = new Queue<TerraAttack>();

        //Sorts the TerraAttacks in the queued list by move priority and then by Terra speed
        for (int i = 0; i < terraAttackList.Count - 1; i++) {
            int highestPriorityAttackIndex = i;
            for (int j = i + 1; j < terraAttackList.Count; j++) {
                if (terraAttackList[highestPriorityAttackIndex].GetMovePriority() > terraAttackList[j].GetMovePriority())
                    continue;
                else if(terraAttackList[highestPriorityAttackIndex].GetMovePriority() == terraAttackList[j].GetMovePriority()) {
                    if (terraAttackList[highestPriorityAttackIndex].GetSpeedPiority() > terraAttackList[j].GetSpeedPiority())
                        continue;
                    else if(terraAttackList[highestPriorityAttackIndex].GetSpeedPiority() == terraAttackList[j].GetSpeedPiority()
                        && terraAttackList[highestPriorityAttackIndex].GetAttackerPosition().GetTerra().GetSpeed() >= terraAttackList[j].GetAttackerPosition().GetTerra().GetSpeed())
                        continue;
                }

                highestPriorityAttackIndex = j;
            }

            if (highestPriorityAttackIndex != i) {
                TerraAttack tempTerraAttack = terraAttackList[i];
                terraAttackList[i] = terraAttackList[highestPriorityAttackIndex];
                terraAttackList[highestPriorityAttackIndex] = tempTerraAttack;
            }
        }

        for(int i = 0; i < terraAttackList.Count; i++)
            terraAttackQueue.Enqueue(terraAttackList[i]);

        return terraAttackQueue;
    }

    public void ProcessTerraAttack(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        if (terraAttack.GetAttackerPosition().GetTerra() == null)
            return;

        Debug.Log(BattleDialog.AttackUsedMsg(terraAttack));
        //*** Terra Attack Declaration Event ***
        battleSystem.InvokeOnAttackDeclaration(terraAttack);

        //If the attack is canceled, remove battle actions on the canceled attack and
        //continue to the next attack
        if (terraAttack.IsCanceled()) {
            terraAttack.GetTerraMoveBase()?.RemoveMoveListeners(battleSystem);
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
            if (defenderPosition.GetTerra() == null) {
                Debug.Log(BattleDialog.ATTACK_FAILED);
                continue;
            }

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

                terraAttack.GetTerraMoveBase().RemoveMoveListeners(battleSystem);
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
