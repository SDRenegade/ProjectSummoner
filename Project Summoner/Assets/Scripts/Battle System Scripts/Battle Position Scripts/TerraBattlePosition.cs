using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    ATK,
    DEF,
    SP_ATK,
    SP_DEF,
    SPD,
    ACC,
    EVA
}

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
    private Dictionary<Stats, StatStages> statStagesMap;
    private BattlePositionState battlePositionState;
    private List<VolatileStatusEffectWrapper> vStatusEffectList;

    public TerraBattlePosition(BattleSide battleSide)
    {
        terra = null;
        this.battleSide = battleSide;
        statStagesMap = new Dictionary<Stats, StatStages> {
            { Stats.ATK, StatStages.NEUTRAL },
            { Stats.DEF, StatStages.NEUTRAL },
            { Stats.SP_ATK, StatStages.NEUTRAL },
            { Stats.SP_DEF, StatStages.NEUTRAL },
            { Stats.SPD, StatStages.NEUTRAL },
            { Stats.ACC, StatStages.NEUTRAL },
            { Stats.EVA, StatStages.NEUTRAL }
        };
        battlePositionState = BattlePositionState.NORMAL;
        vStatusEffectList = new List<VolatileStatusEffectWrapper>();
    }

    public TerraBattlePosition(Terra terra, BattleSide battleSide)
    {
        this.terra = terra;
        this.battleSide = battleSide;
        statStagesMap = new Dictionary<Stats, StatStages> {
            { Stats.ATK, StatStages.NEUTRAL },
            { Stats.DEF, StatStages.NEUTRAL },
            { Stats.SP_ATK, StatStages.NEUTRAL },
            { Stats.SP_DEF, StatStages.NEUTRAL },
            { Stats.SPD, StatStages.NEUTRAL },
            { Stats.ACC, StatStages.NEUTRAL },
            { Stats.EVA, StatStages.NEUTRAL }
        };
        battlePositionState = BattlePositionState.NORMAL;
        vStatusEffectList = new List<VolatileStatusEffectWrapper>();
    }

    public void ResetStatStages()
    {
        foreach(KeyValuePair<Stats, StatStages> entry in statStagesMap)
            statStagesMap[entry.Key] = StatStages.NEUTRAL;
    }

    public Terra GetTerra() { return terra; }

    public void SetTerra(Terra terra) {  this.terra = terra; }

    public BattleSide GetBattleSide() { return battleSide; }

    public void SetBattleSide(BattleSide battleSide) { this.battleSide = battleSide; }

    public StatStages GetStatStage(Stats stat) { return statStagesMap[stat]; }

    public void SetStatStage(Stats stat, StatStages statStage) { statStagesMap[stat] = statStage; }

    public BattlePositionState GetBattlePositionState() { return battlePositionState; }

    public void SetBattlePositionState(BattlePositionState battlePositionState) { this.battlePositionState = battlePositionState; }

    public List<VolatileStatusEffectWrapper> GetVolatileStatusEffectList() { return vStatusEffectList; }

    public bool AddVolatileStatusEffect(VolatileStatusEffectBase vStatusEffectBase, BattleSystem battleSystem)
    {
        foreach(VolatileStatusEffectWrapper vStatusEffectWrapper in vStatusEffectList) {
            if (vStatusEffectWrapper.GetVolatileStatusEffectBase().GetStatusName() == vStatusEffectBase.GetStatusName())
                return false;
        }

        VolatileStatusEffectWrapper vStatusEffectW = new VolatileStatusEffectWrapper(vStatusEffectBase, this);
        vStatusEffectList.Add(vStatusEffectW);
        vStatusEffectW.GetBattleAction()?.AddBattleActions(battleSystem);

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
