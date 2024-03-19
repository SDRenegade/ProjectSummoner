using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleFormat
{
    SINGLE,
    DOUBLE
}

public static class BattleFormatExtension {
    private static Dictionary<BattleFormat, int> numLeadingPositionsByBattleFormat = new Dictionary<BattleFormat, int> {
        {BattleFormat.SINGLE, 1},
        {BattleFormat.DOUBLE, 2}
    };

    public static int NumberOfLeadingPositions(this BattleFormat battleFormat)
    {
        return numLeadingPositionsByBattleFormat[battleFormat];
    }
}
