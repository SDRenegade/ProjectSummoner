using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager
{
    private BattleSystem battleSystem;

    private InitBattleState initBattleState;
    private StartOfTurnBattleState startTurnState;
    private ActionSelectionBattleState actionSelectionState;
    private CombatBattleState damageCalculationState;
    private EndOfTurnBattleState endTurnState;
    private FinishedMatchBattleState finishedMatchState;

    private BattleState currentState;

    public BattleStateManager(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;

        initBattleState = new InitBattleState();
        startTurnState = new StartOfTurnBattleState();
        actionSelectionState = new ActionSelectionBattleState();
        damageCalculationState = new CombatBattleState();
        endTurnState = new EndOfTurnBattleState();
        finishedMatchState = new FinishedMatchBattleState();

        currentState = initBattleState;
        currentState.EnterState(this);
    }

    public void SwitchState(BattleState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public BattleSystem GetBattleSystem() { return battleSystem; }

    public BattleState GetCurrentState() { return currentState; }
    
    public InitBattleState GetInitState() { return initBattleState; }

    public StartOfTurnBattleState GetStartTurnState() { return startTurnState; }

    public ActionSelectionBattleState GetActionSelectionState() { return actionSelectionState; }

    public CombatBattleState GetCombatState() { return damageCalculationState; }

    public EndOfTurnBattleState GetEndTurnState() { return endTurnState; }

    public FinishedMatchBattleState GetFinishedMatchState() {  return finishedMatchState; }
}
