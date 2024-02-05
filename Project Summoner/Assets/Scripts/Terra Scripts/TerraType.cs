using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
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

public enum EffectivenessTypes
{
    NEUTRAL,
    SUPER,
    NOT_VERY,
    IMMUNE
}

public static class TerraTypeExtension
{
    //These are set for convinence, so the entire TypeEffectiveness.<VALUE> doesn't need to be typed for the chart
    private static readonly EffectivenessTypes NEUTRAL = EffectivenessTypes.NEUTRAL;
    private static readonly EffectivenessTypes SUPER = EffectivenessTypes.SUPER;
    private static readonly EffectivenessTypes NOT_VERY = EffectivenessTypes.NOT_VERY;
    private static readonly EffectivenessTypes IMMUNE = EffectivenessTypes.IMMUNE;

    private static readonly Dictionary<EffectivenessTypes, float> typeEffectivenessModifierMap = new Dictionary<EffectivenessTypes, float> {
        {EffectivenessTypes.NEUTRAL, 1f},
        {EffectivenessTypes.SUPER, 2f},
        {EffectivenessTypes.NOT_VERY, 0.5f},
        {EffectivenessTypes.IMMUNE, 0f}
    };

    private static readonly EffectivenessTypes[,] typeEffectivenessChart = new EffectivenessTypes[13, 13] {
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NOT_VERY, NEUTRAL,  NEUTRAL,  NOT_VERY},
        {NEUTRAL,  NEUTRAL,  NOT_VERY, SUPER,    NOT_VERY, NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, NOT_VERY, NOT_VERY, NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NOT_VERY, NOT_VERY, SUPER,    NEUTRAL,  SUPER,    NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  SUPER,    NOT_VERY, NOT_VERY, NEUTRAL,  NOT_VERY, NEUTRAL,  SUPER,    SUPER,    SUPER,    NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, IMMUNE,   SUPER,    NEUTRAL,  SUPER,    NEUTRAL,  NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NOT_VERY, NEUTRAL,  SUPER,    SUPER,    NEUTRAL,  IMMUNE,   SUPER,    SUPER,    NEUTRAL,  NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, NEUTRAL,  IMMUNE,   NOT_VERY, NOT_VERY, NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  SUPER,    SUPER,    NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NOT_VERY, NOT_VERY, NEUTRAL,  SUPER,    NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  NOT_VERY, NOT_VERY, NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NOT_VERY, SUPER,    NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, NEUTRAL,  NEUTRAL,  SUPER,    NEUTRAL,  NOT_VERY, NOT_VERY, NEUTRAL,  NEUTRAL},
        {NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NOT_VERY, NEUTRAL,  NEUTRAL,  NOT_VERY, SUPER},
        {NEUTRAL,  SUPER,    NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  NEUTRAL,  SUPER,    NOT_VERY}
    };

    public static EffectivenessTypes GetTypeEffectiveness(this TerraType attackType, TerraType defendType)
    {
        return typeEffectivenessChart[(int)attackType, (int)defendType];
    }

    public static EffectivenessTypes GetTypeEffectiveness(this TerraType attackType, List<TerraType> defendTypeList)
    {
        EffectivenessTypes effectivenessType;
        float effectivenessModifier = attackType.GetTypeEffectivenessModifier(defendTypeList);

        if(effectivenessModifier <= 0)
            effectivenessType = EffectivenessTypes.IMMUNE;
        else if(effectivenessModifier < 1)
            effectivenessType = EffectivenessTypes.NOT_VERY;
        else if(effectivenessModifier >= 2)
            effectivenessType = EffectivenessTypes.SUPER;
        else
            effectivenessType = EffectivenessTypes.NEUTRAL;

        return effectivenessType;
    }

    public static float GetTypeEffectivenessModifier(this TerraType attackType, TerraType defendType)
    {
        return typeEffectivenessModifierMap[typeEffectivenessChart[(int)attackType, (int)defendType]];
    }

    public static float GetTypeEffectivenessModifier(this TerraType attackType, List<TerraType> defendTypeList)
    {
        float effectivenessMultiplier = 1f;
        foreach(TerraType defendType in defendTypeList)
            effectivenessMultiplier *= GetTypeEffectivenessModifier(attackType, defendType);

        return effectivenessMultiplier;
    }

    public static float GetEffectivenessTypeValue(EffectivenessTypes effectivenessType)
    {
        return typeEffectivenessModifierMap[effectivenessType];
    }
}
