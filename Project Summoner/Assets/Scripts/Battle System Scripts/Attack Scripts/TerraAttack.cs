using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedPriority
{
    LOW = 0,
    NEUTRAL = 1,
    HIGH = 2
}

public class TerraAttack : IBattleSequence
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
        defendersPositionList = new List<TerraBattlePosition>();
        if (defenderPosition != null)
            defendersPositionList.Add(defenderPosition);
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
        this.defendersPositionList = new List<TerraBattlePosition>();
        for(int i = 0; i < defendersPositionList.Count; i++)
            this.defendersPositionList.Add(defendersPositionList[i]);
        this.move = move;
        terraMoveBase = move.GetMoveSO().CreateTerraMoveAction(this);
        movePriority = move.GetMoveSO().GetBaseMovePriority();
        speedPriority = SpeedPriority.NEUTRAL;
        isCharging = move.GetMoveSO().HasChargeTurn();
        isRecharging = false;
        isPersistent = false;
        isCanceled = false;
    }

    public Dictionary<Action, float> GetBattleSequence(BattleStage battleStage, BattleCamera battleCam)
    {
        Dictionary<Action, float> tasksByTime = new Dictionary<Action, float>();
        float sequenceDuration = 0f;

        // Static shot at attacking terra
        tasksByTime.Add(() => {
            Transform terraTransform = battleStage.GetTerraObject(attackerPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);

            battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, true);
        }, sequenceDuration);
        sequenceDuration += 1.25f;

        // Terra attack animation
        tasksByTime.Add(() => {
            Transform terraTransform = battleStage.GetTerraObject(attackerPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);

            battleCam.SetAttackLookAt(terraOffsetPos, terraTransform.eulerAngles, true);
        }, sequenceDuration);
        sequenceDuration += 2f;

        // Target terra damage animation
        for(int i = 0; i < defendersPositionList.Count; i++) {
            tasksByTime.Add(() => {
                Transform terraTransform = battleStage.GetTerraObject(defendersPositionList[i]).transform;
                Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);

                battleCam.SetAttackLookAt(terraOffsetPos, terraTransform.eulerAngles, true);
            }, sequenceDuration);
            sequenceDuration += 2f;
        }

        return tasksByTime;
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

    public void SetSpeedPriority(SpeedPriority speedPriority) { this.speedPriority = speedPriority; }

    public bool IsCharging() { return isCharging; }

    public void SetCharging(bool isCharging) { this.isCharging = isCharging; }

    public bool IsRecharging() { return isRecharging; }

    public void SetRecharging(bool isRecharging) { this.isRecharging = isRecharging; }

    public bool IsPersistent() { return isPersistent; }

    public void SetPersistent(bool isPersistent) { this.isPersistent = isPersistent; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
