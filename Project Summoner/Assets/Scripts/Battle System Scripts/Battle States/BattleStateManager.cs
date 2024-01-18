using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager
{
    private BattleSystem battleSystem;

    private StartOfTurnBattleState startTurnState;
    private ActionSelectionBattleState actionSelectionState;
    private CombatBattleState damageCalculationState;
    private EndOfTurnBattleState endTurnState;
    private FinishedMatchBattleState finishedMatchState;

    private BattleState currentState;

    public BattleStateManager(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;

        startTurnState = new StartOfTurnBattleState();
        actionSelectionState = new ActionSelectionBattleState();
        damageCalculationState = new CombatBattleState();
        endTurnState = new EndOfTurnBattleState();
        finishedMatchState = new FinishedMatchBattleState();

        currentState = startTurnState;
        currentState.EnterState(this);
    }

    public void SwitchState(BattleState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public BattleSystem GetBattleSystem() { return battleSystem; }

    public BattleState GetCurrentState() { return currentState; }

    public StartOfTurnBattleState GetStartTurnState() { return startTurnState; }

    public ActionSelectionBattleState GetActionSelectionState() { return actionSelectionState; }

    public CombatBattleState GetCombatState() { return damageCalculationState; }

    public EndOfTurnBattleState GetEndTurnState() { return endTurnState; }

    public FinishedMatchBattleState GetFinishedMatchBattleState() {  return finishedMatchState; }
}
