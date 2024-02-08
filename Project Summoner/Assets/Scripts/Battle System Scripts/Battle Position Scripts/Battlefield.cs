using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield
{
    private BattleSide primaryBattleSide;
    private BattleSide secondaryBattleSide;
    //private WeatherCondition weatherCondition
    //private BattleTerrain battleTerrain

    public Battlefield(BattleFormat battleFormat, List<Terra> primarySummonerTerraList, Terra secondarySummonerTerra)
    {
        if (primarySummonerTerraList[0] != null)
            primaryBattleSide = new BattleSide(battleFormat, primarySummonerTerraList);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        secondaryBattleSide = new BattleSide(battleFormat, secondarySummonerTerra);
    }

    public Battlefield(BattleFormat battleFormat, List<Terra> primarySummonerTerraList, List<Terra> secondarySummonerTerraList)
    {
        if (primarySummonerTerraList[0] != null)
            primaryBattleSide = new BattleSide(battleFormat, primarySummonerTerraList);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        if (secondarySummonerTerraList[0] != null)
            secondaryBattleSide = new BattleSide(battleFormat, secondarySummonerTerraList);
        else
            Debug.LogError("Secondary Battle Side Party does not have a leading Terra");
    }

    public BattleSide GetPrimaryBattleSide() { return primaryBattleSide; }

    public BattleSide GetSecondaryBattleSide() { return secondaryBattleSide; }
}
