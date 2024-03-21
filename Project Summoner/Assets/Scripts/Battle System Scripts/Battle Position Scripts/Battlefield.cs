using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield
{
    private BattleSide primaryBattleSide;
    private BattleSide secondaryBattleSide;
    //private WeatherCondition weatherCondition
    //private BattleTerrain battleTerrain

    public Battlefield(BattleFormat battleFormat, List<Terra> primaryTerraList, Terra secondaryTerra)
    {
        if (primaryTerraList[0] != null)
            primaryBattleSide = new BattleSide(battleFormat, primaryTerraList, true);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        secondaryBattleSide = new BattleSide(battleFormat, secondaryTerra, false);
    }

    public Battlefield(BattleFormat battleFormat, List<Terra> primaryTerraList, List<Terra> secondaryTerraList)
    {
        if (primaryTerraList[0] != null)
            primaryBattleSide = new BattleSide(battleFormat, primaryTerraList, true);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        if (secondaryTerraList[0] != null)
            secondaryBattleSide = new BattleSide(battleFormat, secondaryTerraList, false);
        else
            Debug.LogError("Secondary Battle Side Party does not have a leading Terra");
    }

    public BattleSide GetPrimaryBattleSide() { return primaryBattleSide; }

    public BattleSide GetSecondaryBattleSide() { return secondaryBattleSide; }
}
