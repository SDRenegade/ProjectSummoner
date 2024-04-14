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

    public void Start()
    {
        for (int i = 0; i < primaryStatusBarList.Count; i++)
            primaryStatusBarList[i].gameObject.SetActive(false);
        for (int i = 0; i < secondaryStatusBarList.Count; i++)
            secondaryStatusBarList[i].gameObject.SetActive(false);
    }

    public void InitStatusBars(BattleFormat battleFormat)
    {
        primaryStatusBarList = new List<TerraBattleStatusBar>();
        secondaryStatusBarList = new List<TerraBattleStatusBar>();
        for(int i = 0; i < battleFormat.NumberOfLeadingPositions(); i++) {
            TerraBattleStatusBar primaryTerraStatusBar = Instantiate(terraStatusBarPrefab).GetComponent<TerraBattleStatusBar>();
            TerraBattleStatusBar secondaryTerraStatusBar = Instantiate(terraStatusBarPrefab).GetComponent<TerraBattleStatusBar>();
            primaryStatusBarList.Add(primaryTerraStatusBar);
            secondaryStatusBarList.Add(secondaryTerraStatusBar);
        }
    }

    public void UpdateTerraStatusBars(Battlefield battlefield)
    {
        TerraBattlePosition[] primaryTerraBattlePositionArr = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositionArr = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();

        for (int i = 0; i < primaryStatusBarList.Count; i++) {
            if (primaryTerraBattlePositionArr[i].GetTerra() == null) {
                primaryStatusBarList[i].gameObject.SetActive(false);
                continue;
            }

            primaryStatusBarList[i].gameObject.SetActive(true);
            primaryStatusBarList[i].UpdateStatusBar(primaryTerraBattlePositionArr[i].GetTerra());
        }
        for (int i = 0; i < secondaryStatusBarList.Count; i++) {
            if (secondaryTerraBattlePositionArr[i].GetTerra() == null) {
                secondaryStatusBarList[i].gameObject.SetActive(false);
                continue;
            }

            secondaryStatusBarList[i].gameObject.SetActive(true);
            secondaryStatusBarList[i].UpdateStatusBar(secondaryTerraBattlePositionArr[i].GetTerra());
        }

    }

    public void HideTerraStatusBars()
    {
        for (int i = 0; i < primaryStatusBarList.Count; i++)
            primaryStatusBarList[i].gameObject.SetActive(false);
        for (int i = 0; i < secondaryStatusBarList.Count; i++)
            secondaryStatusBarList[i].gameObject.SetActive(false);
    }
}
