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
        for (int i = 0; i < queuedTerraAttackList.Count; i++)
            ProcessTerraAttack(queuedTerraAttackList[i], battleSystem);

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

        //If flinched, continue to the next attack
        if (terraAttack.IsFlinched()) {
            terraAttack.GetTerraMoveAction()?.RemoveBattleActions(battleSystem);
            Debug.Log(BattleDialog.FlinchedMsg(terraAttack.GetAttackerPosition().GetTerra()));
            return;
        }
        //If the attack is canceled, continue to the next attack
        if (terraAttack.IsCanceled()) {
            terraAttack.GetTerraMoveAction()?.RemoveBattleActions(battleSystem);
            Debug.Log(BattleDialog.ATTACK_FAILED);
            return;
        }

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

            DamageCalculation(terraAttack, directAttackLogList[i], battleSystem);

            //Accuracy Check. If false, the move misses and we continue to the next defending battle position.
            if (!CombatCalculator.HitCheck(directAttackLogList[i].GetDirectAttackParams())) {
                Debug.Log(BattleDialog.ATTACK_MISSED);
                //*** Attack Missed Event ***
                battleSystem.InvokeOnAttackMissed(directAttackLogList[i]);
                continue;
            }

            directAttackLogList[i].SetSuccessfulHit(true);

            ApplyDamage(directAttackLogList[i]);

            //After damage is calculated and applied we activate the move's post attack effect
            terraAttack.GetTerraMoveAction()?.PostAttackEffect(directAttackLogList[i], battleSystem);

            battleSystem.UpdateTerraStatusBars();

            //--- (Temp) Checks if the defending terra has fainted. If so, the game ends. ---
            if (defenderPosition.GetTerra().GetCurrentHP() <= 0) {
                Debug.Log(BattleDialog.TerraFaintedMsg(defenderPosition.GetTerra()));
                battleSystem.EndBattle();
                return;
            }
        }
    }

    //If the move used is not a status move and the damage step is not canceled, we calculate the damage dealt
    private void DamageCalculation(TerraAttack terraAttack, DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        if (terraAttack.GetMove().GetMoveBase().GetDamageType() == DamageType.STATUS || directAttackLog.GetDirectAttackParams().IsDamageStepCanceled())
            return;

        for (int i = 0; i < directAttackLog.GetDirectAttackParams().GetHitCount(); i++) {
            directAttackLog.SetCrit(CombatCalculator.CriticalHitCheck(directAttackLog.GetDirectAttackParams()));

            int? damage = CombatCalculator.CalculateDamage(directAttackLog.GetDirectAttackParams(), directAttackLog.IsCrit());

            //*** Terra Damage by Terra Event ***
            TerraDamageByTerraEventArgs eventArgs = battleSystem.InvokeOnTerraDamageByTerra(terraAttack, directAttackLog, damage);
            damage = eventArgs.GetDamage();

            if (damage != null)
                directAttackLog.AddDamage(damage);
        }
    }

    //If the damage from the DirectAttackLog is not null, then we apply the damage
    private void ApplyDamage(DirectAttackLog directAttackLog)
    {
        if (directAttackLog.GetDamage() == null)
            return;

        directAttackLog.GetDefenderPosition().GetTerra().TakeDamage((int)directAttackLog.GetDamage());

        if (directAttackLog.IsCrit())
            Debug.Log(BattleDialog.CRITICAL_HIT);

        if (directAttackLog.GetDirectAttackParams().GetHitCount() > 1)
            Debug.Log(BattleDialog.MultiHitMsg(
                directAttackLog.GetDirectAttackParams().GetAttackerPosition().GetTerra(),
                directAttackLog.GetDirectAttackParams().GetHitCount()));

        Debug.Log(BattleDialog.DamageDealtMsg(
            directAttackLog.GetAttackerPosition().GetTerra(),
            directAttackLog.GetDefenderPosition().GetTerra(),
            (int)directAttackLog.GetDamage()));
    }
}
