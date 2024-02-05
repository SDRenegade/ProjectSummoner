using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedPriority
{
    LOW = 0,
    NEUTRAL = 1,
    HIGH = 2
}

public class TerraAttack
{
    private TerraBattlePosition attackerPosition;
    private List<TerraBattlePosition> defendersPositionList;
    private TerraMove move;
    private TerraMoveBase terraMoveBase;
    private MovePriority movePriority;
    private SpeedPriority speedPriority;
    private bool isCharging;
    private bool isRecharging;
    private bool isPersistent;
    private bool isCanceled;

    public TerraAttack(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        defendersPositionList = new List<TerraBattlePosition> { defenderPosition };
        this.move = move;
        terraMoveBase = move.GetMoveSO().CreateTerraMoveAction(this);
        movePriority = move.GetMoveSO().GetBaseMovePriority();
        speedPriority = SpeedPriority.NEUTRAL;
        isCharging = move.GetMoveSO().HasChargeTurn();
        isRecharging = false;
        isPersistent = false;
        isCanceled = false;
    }

    public TerraAttack(TerraBattlePosition attackerPosition, List<TerraBattlePosition> defendersPositionList, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defendersPositionList = defendersPositionList;
        this.move = move;
        terraMoveBase = move.GetMoveSO().CreateTerraMoveAction(this);
        movePriority = move.GetMoveSO().GetBaseMovePriority();
        speedPriority = SpeedPriority.NEUTRAL;
        isCharging = move.GetMoveSO().HasChargeTurn();
        isRecharging = false;
        isPersistent = false;
        isCanceled = false;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public List<TerraBattlePosition> GetDefendersPositionList() { return defendersPositionList; }

    public TerraMove GetMove() {  return move; }

    public void SetMove(TerraMove move)
    {
        this.move = move;
        terraMoveBase = move.GetMoveSO().CreateTerraMoveAction(this);
        movePriority = move.GetMoveSO().GetBaseMovePriority();
    }

    public TerraMoveBase GetTerraMoveBase() {  return terraMoveBase; }

    public MovePriority GetMovePriority() {  return movePriority; }

    public void SetMovePriority(MovePriority movePriority) { this.movePriority = movePriority; }

    public SpeedPriority GetSpeedPiority() { return speedPriority; }

    public void SetSpeedPriority(SpeedPriority speedPiority) { this.speedPriority = speedPiority; }

    public bool IsCharging() { return isCharging; }

    public void SetCharging(bool isCharging) { this.isCharging = isCharging; }

    public bool IsRecharging() { return isRecharging; }

    public void SetRecharging(bool isRecharging) { this.isRecharging = isRecharging; }

    public bool IsPersistent() { return isPersistent; }

    public void SetPersistent(bool isPersistent) { this.isPersistent = isPersistent; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
