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
    private List<TerraSwitch> terraSwitchList;
    //private List<CatchAttemptAction> catchAttemptDieList;
    private bool isAttemptingEscape;

    private TerraAttack pendingTerraAttack;

    public BattleActionManager(BattleSystem battleSystem, int numBattlePositions)
    {
        readyActionList = new bool[numBattlePositions];
        terraActionSelectionQueue = new List<TerraBattlePosition>();
        selectedActionStack = new Stack<BattleAction>();
        attackLog = new List<List<TerraAttack>>();
        terraAttackList = new List<TerraAttack>();
        terraSwitchList = new List<TerraSwitch>();
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

        terraSwitchList.Clear();
        isAttemptingEscape = false;
    }

    public void AddTerraToActionSelectionQueue(TerraBattlePosition terraBattlePosition)
    {
        terraActionSelectionQueue.Add(terraBattlePosition);
    }

    public void AddBattleActionToStack(BattleAction battleAction)
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

    public void PushPendingTerraAttack(TerraBattlePosition targetTerraPosition)
    {
        pendingTerraAttack.GetDefendersPositionList().Add(targetTerraPosition);
        AddBattleActionToStack(new TerraAttackBattleAction(pendingTerraAttack.GetAttackerPosition(), pendingTerraAttack));
        pendingTerraAttack = null;
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

    public TerraBattlePosition GetCurrentTerraActionSelection()
    {
        return (terraActionSelectionQueue.Count > 0) ? terraActionSelectionQueue[0] : null;
    }

    public Stack<BattleAction> GetSelectedActionStack() { return selectedActionStack; }

    public List<List<TerraAttack>> GetAttackLog() { return attackLog; }

    public List<TerraAttack> GetTerraAttackList() { return terraAttackList; }

    public List<TerraSwitch> GetTerraSwitchList() { return terraSwitchList; }

    public bool IsAttemptingEscape() { return isAttemptingEscape; }

    public void SetAttemptingEscape(bool isAttemptingEscape) { this.isAttemptingEscape = isAttemptingEscape; }

    public TerraAttack GetPendingTerraAttack() { return pendingTerraAttack; }

    public void SetPendingTerraAttack(TerraAttack pendingTerraAttack) { this.pendingTerraAttack = pendingTerraAttack; }
}
