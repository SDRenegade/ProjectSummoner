using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerraType
{
    VIS = 0,
    NATURE = 1,
    WATER = 2,
    FIRE = 3,
    ELECTRIC = 4,
    GAIA = 5,
    AVIAN = 6,
    BLIGHT = 7,
    STEEL = 8,
    ICE = 9,
    DARK = 10,
    MYSTIC = 11
}

public static class TerraTypeExtension
{
    private static readonly float[,] typeEffectivenessChart = new float[12, 12] {
        {1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 0.5f, 1f, 1f, 0.5f},
        {1f, 0.5f, 2f, 0.5f, 1f, 2f, 1f, 0.5f, 0.5f, 0.5f, 1f, 1f},
        {1f, 0.5f, 0.5f, 2f, 1f, 2f, 1f, 2f, 1f, 0.5f, 1f, 1f},
        {1f, 2f, 0.5f, 0.5f, 1f, 0.5f, 1f, 2f, 2f, 2f, 1f, 1f},
        {1f, 1f, 2f, 1f, 0.5f, 0f, 2f, 1f, 2f, 1f, 1f, 1f},
        {1f, 0.5f, 1f, 2f, 2f, 1f, 0f, 2f, 2f, 1f, 1f, 1f},
        {1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 1f},
        {1f, 2f, 2f, 1f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 2f, 1f},
        {1f, 1f, 0.5f, 0.5f, 1f, 1f, 1f, 1f, 0.5f, 2f, 1f, 1f},
        {1f, 2f, 1f, 0.5f, 1f, 1f, 2f, 1f, 0.5f, 0.5f, 1f, 1f},
        {1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 0.5f, 2f},
        {2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f}
    };

    public static float GetTypeEffectiveness(this TerraType attackType, TerraType defendType)
    {
        return typeEffectivenessChart[(int)attackType, (int)defendType];
    }

    public static float GetTypeEffectiveness(this TerraType attackType, List<TerraType> defendTypeList)
    {
        float effectivenessMultiplier = 1f;
        foreach(TerraType defendType in defendTypeList)
            effectivenessMultiplier *= GetTypeEffectiveness(attackType, defendType);

        return effectivenessMultiplier;
    }
}
