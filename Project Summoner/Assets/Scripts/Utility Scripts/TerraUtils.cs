using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerraUtils
{
    // Returns true if there is at least one terra in the party that isn't fainted
    public static bool HasLivingPartyMember(List<Terra> terraList)
    {
        bool hasLivingTerra = false;
        for (int i = 0; i < terraList.Count; i++) {
            if (terraList[i].GetCurrentHP() > 0) {
                hasLivingTerra = true;
                break;
            }
        }

        return hasLivingTerra;
    }

    // Returns true if there is at least one benched terra in the party that isn't fainted
    public static bool HasLivingBenchedPartyMember(List<Terra> terraList, BattleFormat battleFormat)
    {
        bool hasLivingBenchTerra = false;
        int leadingTerraPositions = battleFormat.NumberOfLeadingPositions();
        for (int i = leadingTerraPositions; i < terraList.Count; i++) {
            if (terraList[i].GetCurrentHP() > 0) {
                hasLivingBenchTerra = true;
                break;
            }
        }

        return hasLivingBenchTerra;
    }
}
