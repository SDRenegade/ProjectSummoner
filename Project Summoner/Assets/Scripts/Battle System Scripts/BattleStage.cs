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
    private GameObject[] primaryTerraGOArr;
    private GameObject[] secondaryTerraGOArr;

    private float sinOfBattleOriginY;
    private float cosOfBattleOriginY;

    public void Start()
    {
        sinOfBattleOriginY = Mathf.Sin((battlefieldOrigin.eulerAngles.y * Mathf.PI) / 180f);
        cosOfBattleOriginY = Mathf.Cos((battlefieldOrigin.eulerAngles.y * Mathf.PI) / 180f);

        primaryTerraFieldCenterPos = battlefieldOrigin.position;
        primaryTerraFieldCenterPos.x -= opposingTerraSpacing * sinOfBattleOriginY;
        primaryTerraFieldCenterPos.z -= opposingTerraSpacing * cosOfBattleOriginY;
        secondaryTerraFieldCenterPos = battlefieldOrigin.position;
        secondaryTerraFieldCenterPos.x += opposingTerraSpacing * sinOfBattleOriginY;
        secondaryTerraFieldCenterPos.z += opposingTerraSpacing * cosOfBattleOriginY;
        primarySummonerPos = primaryTerraFieldCenterPos;
        primarySummonerPos.x -= summonerTerraSpacing * sinOfBattleOriginY;
        primarySummonerPos.z -= summonerTerraSpacing * cosOfBattleOriginY;
        secondarySummonerPos = secondaryTerraFieldCenterPos;
        secondarySummonerPos.x += summonerTerraSpacing * sinOfBattleOriginY;
        secondarySummonerPos.z += summonerTerraSpacing * cosOfBattleOriginY;

        if (primarySummonerPrefab != null) {
            primarySummonerGO = Instantiate(primarySummonerPrefab);
            primarySummonerGO.transform.position = primarySummonerPos;
            primarySummonerGO.transform.eulerAngles = new Vector3(0f, battlefieldOrigin.eulerAngles.y, 0f);
        }
        if (BattleLoader.GetInstance().GetBattleType() != BattleType.WILD && secondarySummonerPrefab != null) {
            secondarySummonerGO = Instantiate(secondarySummonerPrefab);
            secondarySummonerGO.transform.position = secondarySummonerPos;
            secondarySummonerGO.transform.eulerAngles = new Vector3(0f, battlefieldOrigin.eulerAngles.y - 180f, 0f);
        }

        int numTerraPositions = (BattleLoader.GetInstance().GetBattleFormat() == BattleFormat.SINGLE) ? 1 : 2;
        primaryTerraGOArr = new GameObject[numTerraPositions];
        secondaryTerraGOArr = new GameObject[numTerraPositions];
    }

    public GameObject GetPrimarySummonerGO() { return primarySummonerGO; }

    public GameObject GetSecondarySummonerGO() { return secondarySummonerGO; }

    public GameObject[] GetPrimaryTerraGOArr() { return primaryTerraGOArr; }

    public GameObject[] GetSecondaryTerraGOArr() { return secondaryTerraGOArr; }

    public Vector3 GetTerraPosition(bool isPrimarySide, int positionIndex)
    {
        Vector3 terraPosition = isPrimarySide ? primaryTerraFieldCenterPos : secondaryTerraFieldCenterPos;

        //This could be generalized to account for any number of terra positions
        if(BattleLoader.GetInstance().GetBattleFormat() == BattleFormat.DOUBLE) {
            if (positionIndex < 0 || positionIndex >= primaryTerraGOArr.Length)
                positionIndex = 0;

            float centerSpacing = allyTerraSpacing / 2;
            if (positionIndex == 0)
                centerSpacing = -centerSpacing;

            //Calculating the perpendicular sin and cos of the battle origin Y rotation
            float perpendicularSinOfBattleOriginY = Mathf.Sin(((battlefieldOrigin.eulerAngles.y + 90) * Mathf.PI) / 180f);
            float perpendicularCosOfBattleOriginY = Mathf.Cos(((battlefieldOrigin.eulerAngles.y + 90) * Mathf.PI) / 180f);
            terraPosition.x += centerSpacing * perpendicularSinOfBattleOriginY;
            terraPosition.z += centerSpacing * perpendicularCosOfBattleOriginY;
        }

        return terraPosition;
    }

    public void SetTerraAtPosition(Terra terra, bool isPrimarySide, int positionIndex)
    {
        if (positionIndex >= primaryTerraGOArr.Length) {
            Debug.Log("position index " + positionIndex + " is out of bounds for SetTerraAtPosition in BattleStage.");
            return;
        }

        Vector3 terraPos = GetTerraPosition(isPrimarySide, positionIndex);
        Vector3 terraRot = Vector3.zero;
        terraRot.y = isPrimarySide ? battlefieldOrigin.eulerAngles.y : battlefieldOrigin.eulerAngles.y - 180f;
        GameObject[] terraGOList = isPrimarySide ? primaryTerraGOArr : secondaryTerraGOArr;

        if (terraGOList[positionIndex] != null)
            Destroy(terraGOList[positionIndex]);
        terraGOList[positionIndex] = Instantiate(terra.GetTerraBase().GetTerraGameObject());
        terraGOList[positionIndex].transform.position = terraPos;
        terraGOList[positionIndex].transform.eulerAngles = terraRot;
    }
}
