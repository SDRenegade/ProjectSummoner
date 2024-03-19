using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSide
{
    private TerraBattlePosition[] terraBattlePositionArr;
    //private List<BattleHazard> hazardList;

    public BattleSide(BattleFormat battleFormat, List<Terra> terraList)
    {
        int numBattlePositions = battleFormat.NumberOfLeadingPositions();
        terraBattlePositionArr = new TerraBattlePosition[numBattlePositions];
        for (int i = 0; i < numBattlePositions; i++)
            terraBattlePositionArr[i] = new TerraBattlePosition(this);

        int battlePositionIndex = 0;
        for(int i = 0; i < terraList.Count; i++) {
            if (battlePositionIndex >= terraBattlePositionArr.Length)
                break;

            if (terraList[i] != null && terraList[i].GetCurrentHP() > 0) {
                terraBattlePositionArr[battlePositionIndex].SetTerra(terraList[i]);
                battlePositionIndex++;
            }
        }
    }

    public BattleSide(BattleFormat battleFormat, Terra terra)
    {
        int numBattlePositions = battleFormat.NumberOfLeadingPositions();
        terraBattlePositionArr = new TerraBattlePosition[numBattlePositions];
        for (int i = 0; i < numBattlePositions; i++)
            terraBattlePositionArr[i] = new TerraBattlePosition(this);

        if (terra != null && terra.GetCurrentHP() > 0)
            terraBattlePositionArr[0].SetTerra(terra);
    }

    public void UpdateLeadingTerra(List<Terra> terraList)
    {
        for(int i = 0; i < terraBattlePositionArr.Length; i++) {
            if (i >= terraList.Count)
                break;

            terraBattlePositionArr[i].SetTerra(terraList[i]);
        }
    }

    public TerraBattlePosition[] GetTerraBattlePositionArr() { return terraBattlePositionArr; }
}
