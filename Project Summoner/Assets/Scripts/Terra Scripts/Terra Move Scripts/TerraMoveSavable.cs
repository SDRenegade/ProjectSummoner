using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerraMoveSavable
{
    [SerializeField] private string terraMoveBaseName;
    [SerializeField] private int maxPP;
    [SerializeField] private int currentPP;

    public TerraMoveSavable(TerraMove terraMove)
    {
        terraMoveBaseName = terraMove.GetMoveSO().GetMoveName();
        maxPP = terraMove.GetMaxPP();
        currentPP = terraMove.GetCurrentPP();
    }

    public string GetTerraMoveBaseName() { return terraMoveBaseName; }

    public int GetMaxPP() {  return maxPP; }

    public int GetCurrentPP() { return currentPP; }
}
