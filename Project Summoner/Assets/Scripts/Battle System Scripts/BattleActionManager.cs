using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class BattleActionManager
{
    private bool[] readyActionList;
    private List<TerraBattlePosition> terraActionSelectionQueue;
    private Stack<BattleAction> selectedActionStack;
    private List<List<TerraAttack>> attackLog;
    private List<TerraAttack> terraAttackList;
    //private List<TerraSwitchBattleAction> TerraSwitchList;
    //private SummonerDie nextCatchAttemptDie;
    private bool isAttemptingEscape;

    private TerraAttack pendingTerraAttack; //Might change location

    public BattleActionManager(BattleSystem battleSystem, int numBattlePositions)
    {
        readyActionList = new bool[numBattlePositions];
        terraActionSelectionQueue = new List<TerraBattlePosition>();
        selectedActionStack = new Stack<BattleAction>();
        attackLog = new List<List<TerraAttack>>();
        terraAttackList = new List<TerraAttack>();
        isAttemptingEscape = false;
        pendingTerraAttack = null;

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

    public void ResetActions(BattleSystem battleSystem)
    {
        for(int i = 0; i < readyActionList.Length; i++)
            readyActionList[i] = false;
        //Removes all terra attacks from attack list that are not persistent
        for (int i = terraAttackList.Count - 1; i >= 0; i--) {
            if (terraAttackList[i].IsCharging())
                terraAttackList[i].SetCharging(false);
            else if (!terraAttackList[i].IsPersistent() && !terraAttackList[i].IsRecharging()) {
                terraAttackList[i].GetTerraMoveBase()?.RemoveBattleActions(battleSystem);
                terraAttackList.RemoveAt(i);
            }
        }
        isAttemptingEscape = false;
    }

    public void AddTerraToActionSelectionQueue(TerraBattlePosition terraBattlePosition)
    {
        terraActionSelectionQueue.Add(terraBattlePosition);
    }

    public void AddSelectedActionToStack(BattleAction battleAction)
    {
        if (terraActionSelectionQueue.Count <= 0) {
            Debug.LogError("An action was attempted to be added to the stack without any terra in the action selection queue");
            return;
        }

        terraActionSelectionQueue.RemoveAt(0);
        selectedActionStack.Push(battleAction);
    }

    public bool CancelLastSelectedAction()
    {
        if (selectedActionStack.Count <= 0)
            return false;

        terraActionSelectionQueue.Insert(0, selectedActionStack.Pop().GetTerraBattlePosition());
        RemoveReadyBattlePosition();
        return true;
    }

    public void ProcessSelectedActionStack(BattleSystem battleSystem)
    {
        if (selectedActionStack.Count <= 0)
            return;

        while (selectedActionStack.Count > 0)
            selectedActionStack.Pop().ProcessBattleAction(battleSystem, this);
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

    public bool RemoveReadyBattlePosition()
    {
        for (int i = readyActionList.Length - 1; i  >= 0; i--) {
            if (readyActionList[i] == true) {
                readyActionList[i] = false;
                return true;
            }
        }

        return false;
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

    public TerraBattlePosition GetNextTerraActionSelection()
    {
        return (terraActionSelectionQueue.Count > 0) ? terraActionSelectionQueue[0] : null;
    }

    public List<List<TerraAttack>> GetAttackLog() { return attackLog; }

    public List<TerraAttack> GetTerraAttackList() { return terraAttackList; }

    public void SetTerraAttackList(List<TerraAttack> terraAttackList) { this.terraAttackList = terraAttackList; }

    public bool IsAttemptingEscape() { return isAttemptingEscape; }

    public void SetAttemptingEscape(bool isAttemptingEscape) { this.isAttemptingEscape = isAttemptingEscape; }

    public TerraAttack GetPendingTerraAttack() { return pendingTerraAttack; }

    public void SetPendingTerraAttack(TerraAttack pendingTerraAttack) { this.pendingTerraAttack = pendingTerraAttack; }
}
