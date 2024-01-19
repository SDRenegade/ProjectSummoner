using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield
{
    private BattleSide primaryBattleSide;
    private BattleSide secondaryBattleSide;
    //private WeatherCondition weatherCondition
    //private BattleTerrain battleTerrain

    public Battlefield(TerraParty primarySideParty, Terra secondarySideTerra)
    {
        if (primarySideParty.GetTerraList()[0] != null)
            primaryBattleSide = new BattleSide(1, primarySideParty);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        secondaryBattleSide = new BattleSide(1, secondarySideTerra);
    }

    public Battlefield(TerraParty primarySideParty, TerraParty secondarySideParty)
    {
        if (primarySideParty.GetTerraList()[0] != null)
            primaryBattleSide = new BattleSide(1, primarySideParty);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        if (secondarySideParty.GetTerraList()[0] != null)
            secondaryBattleSide = new BattleSide(1, secondarySideParty);
        else
            Debug.LogError("Secondary Battle Side Party does not have a leading Terra");
    }

    public BattleSide GetPrimaryBattleSide() { return primaryBattleSide; }

    public BattleSide GetSecondaryBattleSide() { return secondaryBattleSide; }
}
