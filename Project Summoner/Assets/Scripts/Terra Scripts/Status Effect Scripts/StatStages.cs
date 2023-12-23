using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatStages
{
    STAGE_NEG_THREE = 0,
    STAGE_NEG_TWO = 1,
    STAGE_NEG_ONE = 2,
    NEUTRAL = 3,
    STAGE_ONE = 4,
    STAGE_TWO = 5,
    STAGE_THREE = 6
}

public static class StatStagesExtension
{
    private static readonly float[] stageMultipliers = new float[7] {
        0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 1.75f
    };

    public static StatStages ToEnum(int value)
    {
        if (typeof(StatStages).IsEnumDefined(value))
            return (StatStages)value;

        return default;
    }

    public static StatStages ChangeStatStage(StatStages statStage, int numStages)
    {
        StatStages newStage;
        int currentStageIndex = (int)statStage;
        if (!typeof(StatStages).IsEnumDefined(currentStageIndex - numStages))
            newStage = (numStages > 0) ? StatStages.STAGE_THREE : StatStages.STAGE_NEG_THREE;
        else
            newStage = (StatStages)(currentStageIndex - numStages);

        return newStage;
    }

    public static StatStages IncrementStage(StatStages statStage)
    {
        int currentStageIndex = (int)statStage;
        return (StatStages)(typeof(StatStages).IsEnumDefined(currentStageIndex + 1) ? currentStageIndex + 1 : currentStageIndex);
    }

    public static StatStages DecrementStage(StatStages statStage)
    {
        int currentStageIndex = (int)statStage;
        return (StatStages)(typeof(StatStages).IsEnumDefined(currentStageIndex - 1) ? currentStageIndex - 1 : currentStageIndex);
    }

    public static float GetStatStageMultiplier(this StatStages stage)
    {
        return stageMultipliers[(int)stage];
    }
}
