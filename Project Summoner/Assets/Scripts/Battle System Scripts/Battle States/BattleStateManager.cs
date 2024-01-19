using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager
{
    private BattleSystem battleSystem;

    private StartTurnBattleState startTurnState;
    private ActionSelectionBattleState actionSelectionState;
    private CombatBattleState damageCalculationState;
    private EndTurnBattleState endTurnState;

    private BattleState currentState;

    public BattleStateManager(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;

        startTurnState = new StartTurnBattleState();
        actionSelectionState = new ActionSelectionBattleState();
        damageCalculationState = new CombatBattleState();
        endTurnState = new EndTurnBattleState();

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

    public StartTurnBattleState GetStartTurnState() { return startTurnState; }

    public ActionSelectionBattleState GetActionSelectionState() { return actionSelectionState; }

    public CombatBattleState GetCombatState() { return damageCalculationState; }

    public EndTurnBattleState GetEndTurnState() { return endTurnState; }
}
