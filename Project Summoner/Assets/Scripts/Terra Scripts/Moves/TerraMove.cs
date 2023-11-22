using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TerraMove
{
    [SerializeField] private TerraMoveBase terraMoveBase;
    [SerializeField] private int maxPP;
    [SerializeField] private int currentPP;

    public TerraMove(TerraMoveBase terraMoveBase)
    {
        this.terraMoveBase = terraMoveBase;
        maxPP = terraMoveBase.GetBasePP();
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveBase terraMoveBase, int maxPP)
    {
        this.terraMoveBase = terraMoveBase;
        this.maxPP = maxPP;
        currentPP = maxPP;
    }

    public TerraMove(TerraMoveSavable terraMoveSavable)
    {
        terraMoveBase = SODatabase.GetInstance().GetTerraMoveByName(terraMoveSavable.GetTerraMoveBaseName());
        maxPP = terraMoveSavable.GetMaxPP();
        currentPP = terraMoveSavable.GetCurrentPP();
    }

    public bool UseMove(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, BattleSystem battleSystem)
    {
        if(currentPP <= 0)
            return false;
        if (!CombatCalculator.HitCheck(attackingTerraPosition, targetTerraPosition, terraMoveBase)) {
            Debug.Log(terraMoveBase.GetMoveName() + " has missed");
            return false;
        }

        terraMoveBase.PostAttackAbility(attackingTerraPosition, targetTerraPosition, battleSystem);
        currentPP--;
        return true;
    }

    public TerraMoveBase GetMoveBase() { return terraMoveBase; }

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
        if (currentPP <= 0)
            return;

        this.currentPP = (currentPP > maxPP) ? maxPP : currentPP;
    }
}
