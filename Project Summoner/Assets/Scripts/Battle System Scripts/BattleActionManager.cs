using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager
{
    private bool[] readyActionList;
    private List<List<TerraAttack>> attackLog;
    private List<TerraAttack> terraAttackList;
    //private List<TerraSwitchBattleAction> TerraSwitchList;
    //private SummonerDie nextCatchAttemptDie;
    private bool attemptEscape;

    public BattleActionManager(BattleSystem battleSystem, int numBattlePositions)
    {
        readyActionList = new bool[numBattlePositions];
        attackLog = new List<List<TerraAttack>>();
        terraAttackList = new List<TerraAttack>();
        attemptEscape = false;

        battleSystem.OnEnteringCombatState += CreateNewTurnLog;
        battleSystem.OnAttackDeclaration += AddAttackLogEntry;
    }

    public void CreateNewTurnLog(object sender, BattleEventArgs eventArgs)
    {
        attackLog.Add(new List<TerraAttack>());
    }

    public void AddAttackLogEntry(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (attackLog.Count == 0)
            return;

        attackLog[attackLog.Count - 1].Add(eventArgs.GetTerraAttack());
    }

    public void ResetActions()
    {
        for(int i = 0; i < readyActionList.Length; i++)
            readyActionList[i] = false;
        //Removes all terra attacks from attack list that are not persistent
        for (int i = terraAttackList.Count - 1; i >= 0; i--) {
            if (!terraAttackList[i].IsPersistent())
                terraAttackList.RemoveAt(i);
        }
        attemptEscape = false;
    }

    //Returns true if all battle positions are ready
    public bool AddReadyBattlePosition()
    {
        for (int i = 0; i < readyActionList.Length; i++) {
            if (readyActionList[i] == false) {
                readyActionList[i] = true;
                break;
            }
        }

        return IsAllBattlePositionsReady();
    }

    public bool IsAllBattlePositionsReady()
    {
        bool isReady = true;
        for (int i = 0; i < readyActionList.Length; i++) {
            if (readyActionList[i] == false) {
                isReady = false;
                break;
            }
        }

        return isReady;
    }

    public List<TerraAttack> GetCurrentTurnLog()
    {
        if (attackLog.Count == 0)
            return null;

        return attackLog[attackLog.Count - 1];
    }

    public List<TerraAttack> GetPreviousTurnLog()
    {
        if (attackLog.Count <= 1)
            return null;

        return attackLog[attackLog.Count - 2];
    }

    public List<List<TerraAttack>> GetAttackLog() { return attackLog; }

    public List<TerraAttack> GetTerraAttackList() { return terraAttackList; }

    public void SetTerraAttackList(List<TerraAttack> terraAttackList) { this.terraAttackList = terraAttackList; }

    public bool GetIsAttemptEscape() { return attemptEscape; }

    public void SetAttemptEscape(bool attemptEscape) { this.attemptEscape = attemptEscape; }
}
