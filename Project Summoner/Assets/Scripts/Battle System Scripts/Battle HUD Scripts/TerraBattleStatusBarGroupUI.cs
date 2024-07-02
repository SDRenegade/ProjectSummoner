using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraBattleStatusBarGroupUI : MonoBehaviour
{
    [SerializeField] private GameObject terraStatusBarPrefab;
    [SerializeField] private GameObject primaryStatusBarPanel;
    [SerializeField] private GameObject secondaryStatusBarPanel;
    private List<TerraBattleStatusBar> primaryStatusBarList;
    private List<TerraBattleStatusBar> secondaryStatusBarList;

    public void InitStatusBarGroup(BattleFormat battleFormat, Battlefield battlefield)
    {
        primaryStatusBarList = new List<TerraBattleStatusBar>();
        secondaryStatusBarList = new List<TerraBattleStatusBar>();
        for(int i = 0; i < battleFormat.NumberOfLeadingPositions(); i++) {
            GameObject primaryTerraStatusBarObject = Instantiate(terraStatusBarPrefab);
            primaryTerraStatusBarObject.transform.SetParent(primaryStatusBarPanel.transform);
            primaryStatusBarList.Add(primaryTerraStatusBarObject.GetComponent<TerraBattleStatusBar>());
            GameObject secondaryTerraStatusBarObject = Instantiate(terraStatusBarPrefab);
            secondaryTerraStatusBarObject.transform.SetParent(secondaryStatusBarPanel.transform);
            secondaryStatusBarList.Add(secondaryTerraStatusBarObject.GetComponent<TerraBattleStatusBar>());
        }

        StaticUpdateAllStatusBars(battlefield);
        HideStatusBars();
    }

    public void StaticUpdateStatusBar(TerraBattlePosition terraBattlePosition)
    {
        TerraBattleStatusBar statusBar = terraBattlePosition.IsPrimarySide() ?
            primaryStatusBarList[terraBattlePosition.GetBattlePositionIndex()] :
            secondaryStatusBarList[terraBattlePosition.GetBattlePositionIndex()];

        if (terraBattlePosition.GetTerra() != null) {
            statusBar.StaticUpdateStatusBar(terraBattlePosition.GetTerra());
            statusBar.gameObject.SetActive(true);
        }
        else
            statusBar.gameObject.SetActive(false);
    }

    public void DynamicUpdateStatusBar(TerraBattlePosition terraBattlePosition, Terra terra)
    {
        TerraBattleStatusBar statusBar = terraBattlePosition.IsPrimarySide() ?
            primaryStatusBarList[terraBattlePosition.GetBattlePositionIndex()] :
            secondaryStatusBarList[terraBattlePosition.GetBattlePositionIndex()];

        if (terra != null) {
            statusBar.DynamicUpdateStatusBar(terra);
            statusBar.gameObject.SetActive(true);
        }
        else
            statusBar.gameObject.SetActive(false);
    }

    public void StaticUpdateAllStatusBars(Battlefield battlefield)
    {
        TerraBattlePosition[] primaryTerraBattlePositionArr = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositionArr = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();

        for (int i = 0; i < primaryStatusBarList.Count; i++) {
            if (primaryTerraBattlePositionArr[i].GetTerra() == null) {
                primaryStatusBarList[i].gameObject.SetActive(false);
                continue;
            }

            primaryStatusBarList[i].StaticUpdateStatusBar(primaryTerraBattlePositionArr[i].GetTerra());
            primaryStatusBarList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < secondaryStatusBarList.Count; i++) {
            if (secondaryTerraBattlePositionArr[i].GetTerra() == null) {
                secondaryStatusBarList[i].gameObject.SetActive(false);
                continue;
            }

            secondaryStatusBarList[i].StaticUpdateStatusBar(secondaryTerraBattlePositionArr[i].GetTerra());
            secondaryStatusBarList[i].gameObject.SetActive(true);
        }
    }

    public void ShowSingleStatusBar(int positionIndex, bool isPrimarySide)
    {
        HideStatusBars();
        TerraBattleStatusBar shownStatusBar = isPrimarySide ? primaryStatusBarList[positionIndex] : secondaryStatusBarList[positionIndex];
        shownStatusBar.gameObject.SetActive(true);
    }

    public void HideStatusBars()
    {
        for (int i = 0; i < primaryStatusBarList.Count; i++)
            primaryStatusBarList[i].gameObject.SetActive(false);
        for (int i = 0; i < secondaryStatusBarList.Count; i++)
            secondaryStatusBarList[i].gameObject.SetActive(false);
    }
}
