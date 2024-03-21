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
    private List<VolatileStatusEffectBase> vStatusEffectList;

    public TerraBattlePosition(BattleSide battleSide)
    {
        this.battleSide = battleSide;
        terra = null;
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
        vStatusEffectList = new List<VolatileStatusEffectBase>();
    }

    public TerraBattlePosition(Terra terra, BattleSide battleSide)
    {
        this.battleSide = battleSide;
        this.terra = terra;
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
        vStatusEffectList = new List<VolatileStatusEffectBase>();
    }

    public void ResetStatStages()
    {
        foreach(KeyValuePair<Stats, StatStages> entry in statStagesMap)
            statStagesMap[entry.Key] = StatStages.NEUTRAL;
    }

    public BattleSide GetBattleSide() { return battleSide; }

    public Terra GetTerra() { return terra; }

    public void SetTerra(Terra terra) {  this.terra = terra; }

    public StatStages GetStatStage(Stats stat) { return statStagesMap[stat]; }

    public void SetStatStage(Stats stat, StatStages statStage) { statStagesMap[stat] = statStage; }

    public BattlePositionState GetBattlePositionState() { return battlePositionState; }

    public void SetBattlePositionState(BattlePositionState battlePositionState) { this.battlePositionState = battlePositionState; }

    public List<VolatileStatusEffectBase> GetVolatileStatusEffectList() { return vStatusEffectList; }

    public bool HasVolatileStatusEffect(VolatileStatusEffectSO vStatusEffectSO)
    {
        foreach(VolatileStatusEffectBase vStatusEffect in vStatusEffectList) {
            if (vStatusEffect.GetVolatileStatusEffectSO().GetStatusName() == vStatusEffectSO.GetStatusName())
                return true;
        }

        return false;
    }

    public bool AddVolatileStatusEffect(VolatileStatusEffectBase vStatusEffectBase, BattleSystem battleSystem)
    {
        foreach (VolatileStatusEffectBase vStatusEffect in vStatusEffectList) {
            if (vStatusEffect.GetVolatileStatusEffectSO().GetStatusName() == vStatusEffectBase.GetVolatileStatusEffectSO().GetStatusName())
                return false;
        }

        vStatusEffectList.Add(vStatusEffectBase);
        vStatusEffectBase.AddVolatileStatusEffectListeners(battleSystem);

        return true;
    }

    public bool RemoveVolatileStatusEffect(VolatileStatusEffectSO vStatusEffectSO, BattleSystem battleSystem)
    {
        for(int i = vStatusEffectList.Count - 1; i >= 0; i--) {
            if (vStatusEffectList[i].GetVolatileStatusEffectSO().GetStatusName() == vStatusEffectSO.GetStatusName()) {
                vStatusEffectList[i].RemoveVolatileStatusEffectListeners(battleSystem);
                vStatusEffectList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public bool ContainsVolatileStatusEffect(VolatileStatusEffectSO vStatusEffectSO)
    {
        foreach (VolatileStatusEffectBase vStatusEffect in vStatusEffectList) {
            if (vStatusEffect.GetVolatileStatusEffectSO().GetStatusName() == vStatusEffectSO.GetStatusName())
                return true;
        }

        return false;
    }
}
