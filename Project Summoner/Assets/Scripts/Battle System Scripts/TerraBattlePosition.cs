using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //private List<MinorStatusEffect> minorStatusEffectList;

    public TerraBattlePosition(Terra terra, BattleSide battleSide)
    {
        this.terra = terra;
        this.battleSide = battleSide;
        ResetStatStages();
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

}
