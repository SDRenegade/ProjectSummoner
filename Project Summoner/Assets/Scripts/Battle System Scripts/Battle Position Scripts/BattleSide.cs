using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSide
{
    private TerraBattlePosition[] terraBattlePositionArr;
    //private List<BattleHazard> hazardList;

    public BattleSide(int numBattlePositions, TerraParty terraParty)
    {
        terraBattlePositionArr = new TerraBattlePosition[numBattlePositions];
        for (int i = 0; i < numBattlePositions; i++)
            terraBattlePositionArr[i] = new TerraBattlePosition(this);

        int battlePositionIndex = 0;
        for(int i = 0; i < terraParty.GetTerraList().Count; i++) {
            if (battlePositionIndex >= terraBattlePositionArr.Length)
                break;

            if (terraParty.GetTerraList()[i] != null && terraParty.GetTerraList()[i].GetCurrentHP() > 0) {
                terraBattlePositionArr[battlePositionIndex].SetTerra(terraParty.GetTerraList()[i]);
                battlePositionIndex++;
            }
        }
    }

    public BattleSide(int numBattlePositions, Terra terra)
    {
        terraBattlePositionArr = new TerraBattlePosition[numBattlePositions];
        for (int i = 0; i < numBattlePositions; i++)
            terraBattlePositionArr[i] = new TerraBattlePosition(this);

        if (terra != null && terra.GetCurrentHP() > 0)
            terraBattlePositionArr[0].SetTerra(terra);
    }

    public TerraBattlePosition[] GetTerraBattlePositionArr() { return terraBattlePositionArr; }
}
