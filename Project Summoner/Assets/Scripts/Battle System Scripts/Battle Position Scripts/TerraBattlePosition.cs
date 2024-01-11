using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattlePositionState
{
    NORMAL,
    DIGGING,
    FLYING
}

public class TerraBattlePosition
{
    private BattleSide battleSide;
    private Terra terra;
    private StatStages attackStage;
    private StatStages defenceStage;
    private StatStages spAttackStage;
    private StatStages spDefenceStage;
    private StatStages speedStage;
    private StatStages accuracyStage;
    private StatStages evasivenessStage;
    private BattlePositionState battlePositionState;
    private List<VolatileStatusEffectWrapper> vStatusEffectList;

    public TerraBattlePosition(BattleSide battleSide)
    {
        terra = null;
        this.battleSide = battleSide;
        ResetStatStages();
        battlePositionState = BattlePositionState.NORMAL;
        vStatusEffectList = new List<VolatileStatusEffectWrapper>();
    }

    public TerraBattlePosition(Terra terra, BattleSide battleSide)
    {
        this.terra = terra;
        this.battleSide = battleSide;
        ResetStatStages();
        battlePositionState = BattlePositionState.NORMAL;
        vStatusEffectList = new List<VolatileStatusEffectWrapper>();
    }

    public void ResetStatStages()
    {
        attackStage = StatStages.NEUTRAL;
        defenceStage = StatStages.NEUTRAL;
        spAttackStage = StatStages.NEUTRAL;
        spDefenceStage = StatStages.NEUTRAL;
        speedStage = StatStages.NEUTRAL;
        accuracyStage = StatStages.NEUTRAL;
        evasivenessStage = StatStages.NEUTRAL;
    }

    public Terra GetTerra() { return terra; }

    public void SetTerra(Terra terra) {  this.terra = terra; }

    public BattleSide GetBattleSide() { return battleSide; }

    public void SetBattleSide(BattleSide battleSide) { this.battleSide = battleSide; }

    public StatStages GetAttackStage() { return attackStage; }

    public void SetAttackStage(StatStages statStage) { attackStage = statStage; }

    public StatStages GetDefenceStage() { return defenceStage; }

    public void SetDefenceStage(StatStages statStage) { defenceStage = statStage; }

    public StatStages GetSpAttackStage() { return spAttackStage; }

    public void SetSpAttackStage(StatStages statStage) { spAttackStage = statStage; }

    public StatStages GetSpDefenceStage() { return spDefenceStage; }

    public void SetSpDefenceStage(StatStages statStage) { spDefenceStage = statStage; }

    public StatStages GetSpeedStage() { return speedStage; }

    public void SetSpeedStage(StatStages statStage) { speedStage = statStage; }

    public StatStages GetAccuracyStage() { return accuracyStage; }

    public void SetAccuracyStage(StatStages statStage) {  accuracyStage = statStage; }

    public StatStages GetEvasivenessStage() { return evasivenessStage; }

    public void SetEvasivenessStage(StatStages statStage) {  evasivenessStage = statStage; }

    public BattlePositionState GetBattlePositionState() { return battlePositionState; }

    public void SetBattlePositionState(BattlePositionState battlePositionState) { this.battlePositionState = battlePositionState; }

    public List<VolatileStatusEffectWrapper> GetVolatileStatusEffectList() { return vStatusEffectList; }

    public bool AddVolatileStatusEffect(VolatileStatusEffectBase vStatusEffectBase, BattleSystem battleSystem)
    {
        foreach(VolatileStatusEffectWrapper vStatusEffectWrapper in vStatusEffectList) {
            if (vStatusEffectWrapper.GetVolatileStatusEffectBase().GetStatusName() == vStatusEffectBase.GetStatusName())
                return false;
        }

        VolatileStatusEffectWrapper newVolatileStatusEffectWrapper = new VolatileStatusEffectWrapper(vStatusEffectBase, this);
        vStatusEffectList.Add(new VolatileStatusEffectWrapper(vStatusEffectBase, this));
        newVolatileStatusEffectWrapper.AddVolatileStatusEffectBattleAction(this, battleSystem);

        return true;
    }

    public bool RemoveVolatileStatusEffect(VolatileStatusEffectBase vStatusEffectBase)
    {
        for(int i = vStatusEffectList.Count - 1; i >= 0; i--) {
            if (vStatusEffectList[i].GetVolatileStatusEffectBase().GetStatusName() == vStatusEffectBase.GetStatusName()) {
                vStatusEffectList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }
}
