using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TerraMove
{
    [SerializeField] private TerraMoveBase moveBase;
    [SerializeField] private int maxPP;
    [SerializeField] private int currentPP;

    public TerraMove(TerraMoveBase moveBase)
    {
        this.moveBase = moveBase;
        maxPP = moveBase.GetBasePP();
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveBase moveBase, int maxPP)
    {
        this.moveBase = moveBase;
        this.maxPP = maxPP;
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveSavable terraMoveSavable)
    {
        moveBase = SODatabase.GetInstance().GetTerraMoveByName(terraMoveSavable.GetTerraMoveBaseName());
        maxPP = terraMoveSavable.GetMaxPP();
        currentPP = terraMoveSavable.GetCurrentPP();
    }

    public TerraMoveBase GetMoveBase() { return moveBase; }

    public int GetMaxPP() { return maxPP; }

    public void SetMaxPP(int maxPP)
    {
        if (maxPP <= 0)
            return;

        this.maxPP = maxPP;
        if(currentPP > maxPP)
            currentPP = maxPP;
    }

    public int GetCurrentPP() {  return currentPP; }

    public void SetCurrentPP(int currentPP)
    {
        if (currentPP < 0)
            return;

        this.currentPP = (currentPP > maxPP) ? maxPP : currentPP;
    }

    public override string ToString()
    {
        return moveBase.GetMoveName();
    }
}
