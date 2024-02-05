using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TerraMove
{
    [SerializeField] private TerraMoveSO moveSO;
    [SerializeField] private int maxPP;
    [SerializeField] private int currentPP;

    public TerraMove(TerraMoveSO moveSO)
    {
        this.moveSO = moveSO;
        maxPP = moveSO.GetBasePP();
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveSO moveSO, int maxPP)
    {
        this.moveSO = moveSO;
        this.maxPP = maxPP;
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveSavable terraMoveSavable)
    {
        moveSO = SODatabase.GetInstance().GetTerraMoveByName(terraMoveSavable.GetTerraMoveBaseName());
        maxPP = terraMoveSavable.GetMaxPP();
        currentPP = terraMoveSavable.GetCurrentPP();
    }

    public TerraMoveSO GetMoveSO() { return moveSO; }

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

    public void SetCurrentPP(int currentPP) { this.currentPP = Mathf.Clamp(currentPP, 0, maxPP); }

    public void RecoverPP(int num)
    {
        int newCurrentPP = currentPP + num;
        currentPP = Mathf.Clamp(newCurrentPP, 0, maxPP);
    }

    public override string ToString() { return moveSO.GetMoveName(); }
}
