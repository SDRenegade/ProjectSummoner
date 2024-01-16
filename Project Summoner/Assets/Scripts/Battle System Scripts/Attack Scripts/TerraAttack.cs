using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttack
{
    private TerraBattlePosition attackerPosition;
    private List<TerraBattlePosition> defendersPositionList;
    private TerraMove move;
    private TerraMoveAction terraMoveAction;
    private MovePriority movePriority;
    private bool isPersistent;
    private bool isCanceled;

    public TerraAttack(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        defendersPositionList = new List<TerraBattlePosition> { defenderPosition };
        this.move = move;
        terraMoveAction = move.GetMoveBase().CreateTerraMoveAction(this);
        movePriority = move.GetMoveBase().GetBaseMovePriority();
        isPersistent = false;
        isCanceled = false;
    }

    public TerraAttack(TerraBattlePosition attackerPosition, List<TerraBattlePosition> defendersPositionList, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defendersPositionList = defendersPositionList;
        this.move = move;
        terraMoveAction = move.GetMoveBase().CreateTerraMoveAction(this);
        movePriority = move.GetMoveBase().GetBaseMovePriority();
        isPersistent = false;
        isCanceled = false;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public List<TerraBattlePosition> GetDefendersPositionList() { return defendersPositionList; }

    public TerraMove GetMove() {  return move; }

    public void SetMove(TerraMove move)
    {
        this.move = move;
        terraMoveAction = move.GetMoveBase().CreateTerraMoveAction(this);
        movePriority = move.GetMoveBase().GetBaseMovePriority();
    }

    public TerraMoveAction GetTerraMoveAction() {  return terraMoveAction; }

    public MovePriority GetMovePriority() {  return movePriority; }

    public void SetMovePriority(MovePriority movePriority) { this.movePriority = movePriority; }

    public bool IsPersistent() { return isPersistent; }

    public void SetPersistent(bool isPersistent) { this.isPersistent = isPersistent; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
