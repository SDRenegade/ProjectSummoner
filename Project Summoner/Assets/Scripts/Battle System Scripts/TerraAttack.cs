using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraAttack
{
    private TerraBattlePosition attackingTerraPosition;
    private List<TerraBattlePosition> targetTerraPositionList;
    private TerraMove move;
    private bool isCancelled;

    public TerraAttack(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, TerraMove move)
    {
        this.attackingTerraPosition = attackingTerraPosition;
        targetTerraPositionList = new List<TerraBattlePosition> { targetTerraPosition };
        this.move = move;
        isCancelled = false;
    }

    public TerraAttack(TerraBattlePosition attackingTerraPosition, List<TerraBattlePosition> targetTerraPositionList, TerraMove move)
    {
        this.attackingTerraPosition = attackingTerraPosition;
        this.targetTerraPositionList = targetTerraPositionList;
        this.move = move;
        isCancelled = false;
    }

    public TerraBattlePosition GetAttackingTerraPosition() { return attackingTerraPosition; }

    public List<TerraBattlePosition> GetTargetTerraPositionList() { return targetTerraPositionList; }

    public TerraMove GetMove() {  return move; }

    public bool IsCancelled() { return isCancelled; }

    public void SetCancelled(bool isCancelled) { this.isCancelled = isCancelled; }
}
