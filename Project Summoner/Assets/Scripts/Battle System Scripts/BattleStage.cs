using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStage : MonoBehaviour
{
    [SerializeField] private Transform battlefieldOrigin;
    [SerializeField] private float opposingTerraSpacing;
    [SerializeField] private float allyTerraSpacing;
    [SerializeField] private float summonerTerraSpacing;
    [SerializeField] private GameObject primarySummonerPrefab;
    [SerializeField] private GameObject secondarySummonerPrefab;

    private Vector3 primaryTerraFieldCenterPos;
    private Vector3 secondaryTerraFieldCenterPos;
    private Vector3 primarySummonerPos;
    private Vector3 secondarySummonerPos;
    private GameObject primarySummonerGO;
    private GameObject secondarySummonerGO;
    private List<GameObject> primaryTerraGOList;
    private List<GameObject> secondaryTerraGOList;

    public void Start()
    {
        primaryTerraFieldCenterPos = battlefieldOrigin.position;
        primaryTerraFieldCenterPos.z -= opposingTerraSpacing;
        secondaryTerraFieldCenterPos = battlefieldOrigin.position;
        secondaryTerraFieldCenterPos.z += opposingTerraSpacing;

        primarySummonerPos = primaryTerraFieldCenterPos;
        primarySummonerPos.z -= summonerTerraSpacing;
        secondarySummonerPos = secondaryTerraFieldCenterPos;
        secondarySummonerPos.z += summonerTerraSpacing;

        if (primarySummonerPrefab != null) {
            primarySummonerGO = Instantiate(primarySummonerPrefab);
            primarySummonerGO.transform.position = primarySummonerPos;
        }
        if (secondarySummonerPrefab != null) {
            secondarySummonerGO = Instantiate(secondarySummonerPrefab);
            secondarySummonerGO.transform.position = secondarySummonerPos;
        }

        primaryTerraGOList = new List<GameObject> { null };
        secondaryTerraGOList = new List<GameObject> { null };
        if(BattleLoader.GetInstance().GetBattleFormat() == BattleFormat.DOUBLE) {
            primaryTerraGOList.Add(null);
            secondaryTerraGOList.Add(null);
        }
    }

    public GameObject GetPrimarySummonerGO() { return primarySummonerGO; }

    public GameObject GetSecondarySummonerGO() { return secondarySummonerGO; }

    public List<GameObject> GetPrimaryTerraGOList() { return primaryTerraGOList; }

    public List<GameObject> GetSecondaryTerraGOList() { return secondaryTerraGOList; }

    public Vector3 GetTerraPosition(BattleFormat battleFormat, bool isPrimarySide, int positionIndex)
    {
        Vector3 terraPosition = isPrimarySide ? primaryTerraFieldCenterPos : secondaryTerraFieldCenterPos;

        //This could be generalized to account for any number of terra positions
        if(battleFormat == BattleFormat.DOUBLE) {
            if (positionIndex < 0 || positionIndex > 1)
                positionIndex = 0;

            float centerSpacing = allyTerraSpacing / 2;
            if (positionIndex == 0)
                centerSpacing = -centerSpacing;
            if(!isPrimarySide)
                centerSpacing = -centerSpacing;

            terraPosition.x += centerSpacing;
        }

        return terraPosition;
    }

    public void SetTerraAtPosition(Terra terra, BattleFormat battleFormat, bool isPrimarySide, int positionIndex)
    {
        Vector3 terraPos = GetTerraPosition(battleFormat, isPrimarySide, positionIndex);
        List<GameObject> terraGOList = isPrimarySide ? primaryTerraGOList : secondaryTerraGOList;

        if (terraGOList[positionIndex] != null)
            Destroy(terraGOList[positionIndex]);
        terraGOList[positionIndex] = Instantiate(terra.GetTerraBase().GetTerraGameObject());
        terraGOList[positionIndex].transform.position = terraPos;
    }
}
