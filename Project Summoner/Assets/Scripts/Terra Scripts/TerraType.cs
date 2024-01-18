using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerraType
{
    TYPELESS = 0,
    VIS = 1,
    NATURE = 2,
    WATER = 3,
    FIRE = 4,
    ELECTRIC = 5,
    GAIA = 6,
    AVIAN = 7,
    BLIGHT = 8,
    STEEL = 9,
    ICE = 10,
    DARK = 11,
    MYSTIC = 12
}

public static class TerraTypeExtension
{
    private static readonly float[,] typeEffectivenessChart = new float[13, 13] {
        {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f},
        {1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 0.5f, 1f, 1f, 0.5f},
        {1f, 1f, 0.5f, 2f, 0.5f, 1f, 2f, 1f, 0.5f, 0.5f, 0.5f, 1f, 1f},
        {1f, 1f, 0.5f, 0.5f, 2f, 1f, 2f, 1f, 2f, 1f, 0.5f, 1f, 1f},
        {1f, 1f, 2f, 0.5f, 0.5f, 1f, 0.5f, 1f, 2f, 2f, 2f, 1f, 1f},
        {1f, 1f, 1f, 2f, 1f, 0.5f, 0f, 2f, 1f, 2f, 1f, 1f, 1f},
        {1f, 1f, 0.5f, 1f, 2f, 2f, 1f, 0f, 2f, 2f, 1f, 1f, 1f},
        {1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 1f},
        {1f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 2f, 1f},
        {1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 1f, 1f, 0.5f, 2f, 1f, 1f},
        {1f, 1f, 2f, 1f, 0.5f, 1f, 1f, 2f, 1f, 0.5f, 0.5f, 1f, 1f},
        {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 0.5f, 2f},
        {1f, 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f}
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
