using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttack
{
    private TerraBattlePosition attackerPosition;
    private List<TerraBattlePosition> defendersPositionList;
    private TerraMove move;
    private MovePriority movePriority;
    private bool isFlinched;
    private bool isCanceled;

    public TerraAttack(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        defendersPositionList = new List<TerraBattlePosition> { defenderPosition };
        this.move = move;
        movePriority = move.GetMoveBase().GetBaseMovePriority();
        isFlinched = false;
        isCanceled = false;
    }

    public TerraAttack(TerraBattlePosition attackerPosition, List<TerraBattlePosition> defendersPositionList, TerraMove move)
    {
        this.attackerPosition = attackerPosition;
        this.defendersPositionList = defendersPositionList;
        this.move = move;
        movePriority = move.GetMoveBase().GetBaseMovePriority();
        isCanceled = false;
    }

    public TerraBattlePosition GetAttackerPosition() { return attackerPosition; }

    public List<TerraBattlePosition> GetDefendersPositionList() { return defendersPositionList; }

    public TerraMove GetMove() {  return move; }

    public MovePriority GetMovePriority() {  return movePriority; }

    public void SetMovePriority(MovePriority movePriority) { this.movePriority = movePriority; }

    public bool IsFlinched() { return isFlinched; }

    public void SetFlinched(bool isFlinched) { this.isFlinched = isFlinched; }

    public bool IsCanceled() { return isCanceled; }

    public void SetIsCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
