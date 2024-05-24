using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStage : MonoBehaviour
{
    [SerializeField] private Transform battlefieldOrigin;
    [Header("Spacing")]
    [SerializeField] private Vector3 opposingTerraSpacing;
    [SerializeField] private Vector3 allyTerraSpacing;
    [SerializeField] private Vector3 summonerTerraSpacing;
    [Header("Prefabs")]
    [SerializeField] private GameObject primarySummonerPrefab;
    [SerializeField] private GameObject secondarySummonerPrefab;

    private Vector3 primaryTerraFieldCenterPos;
    private Vector3 secondaryTerraFieldCenterPos;
    private Vector3 primarySummonerPos;
    private Vector3 secondarySummonerPos;
    private GameObject primarySummonerGO;
    private GameObject secondarySummonerGO;
    private Dictionary<TerraBattlePosition, GameObject> terraObjectByPosition;

    public void Start()
    {
        terraObjectByPosition = new Dictionary<TerraBattlePosition, GameObject>();

        Quaternion battlefieldRotation = Quaternion.Euler(0, battlefieldOrigin.eulerAngles.y, 0).normalized;

        primaryTerraFieldCenterPos = battlefieldOrigin.position - (battlefieldRotation * opposingTerraSpacing);
        secondaryTerraFieldCenterPos = battlefieldOrigin.position + (battlefieldRotation * opposingTerraSpacing);
        primarySummonerPos = primaryTerraFieldCenterPos - (battlefieldRotation * summonerTerraSpacing);
        secondarySummonerPos = secondaryTerraFieldCenterPos + (battlefieldRotation * summonerTerraSpacing);
    }

    public void InitBattleStage(List<TerraBattlePosition> battlePositionList)
    {
        if (primarySummonerPrefab != null) {
            primarySummonerGO = Instantiate(primarySummonerPrefab);
            primarySummonerGO.transform.position = primarySummonerPos;
            primarySummonerGO.transform.eulerAngles = new Vector3(0f, battlefieldOrigin.eulerAngles.y, 0f);
        }
        if (BattleLoader.GetInstance().GetBattleType() != BattleType.Wild && secondarySummonerPrefab != null) {
            secondarySummonerGO = Instantiate(secondarySummonerPrefab);
            secondarySummonerGO.transform.position = secondarySummonerPos;
            secondarySummonerGO.transform.eulerAngles = new Vector3(0f, battlefieldOrigin.eulerAngles.y - 180f, 0f);
        }

        for(int i = 0; i < battlePositionList.Count; i++) {
            terraObjectByPosition.Add(battlePositionList[i], null);
            if (battlePositionList[i].GetTerra() != null)
                SetTerraAtPosition(battlePositionList[i]);
        }
    }

    public GameObject GetPrimarySummonerGO() { return primarySummonerGO; }

    public GameObject GetSecondarySummonerGO() { return secondarySummonerGO; }

    public GameObject GetTerraObject(TerraBattlePosition battlePosition)
    {
        return terraObjectByPosition.ContainsKey(battlePosition) ? terraObjectByPosition[battlePosition] : null;
    }

    public Vector3? GetTerraPosition(TerraBattlePosition battlePosition)
    {
        if (!terraObjectByPosition.ContainsKey(battlePosition)) {
            Debug.LogWarning("The provided terra battle position was not found in BattleStage");
            return null;
        }

        Vector3 terraPosition = battlePosition.IsPrimarySide() ? primaryTerraFieldCenterPos : secondaryTerraFieldCenterPos;
        //This could be generalized to account for any number of terra positions
        if(BattleLoader.GetInstance().GetBattleFormat() == BattleFormat.DOUBLE) {
            Vector3 centerSpacing = allyTerraSpacing / 2;
            if (battlePosition.GetBattlePositionIndex() == 0)
                centerSpacing = -centerSpacing;

            Quaternion battlefieldRotation = Quaternion.Euler(0, battlefieldOrigin.eulerAngles.y, 0).normalized;
            terraPosition += battlefieldRotation * centerSpacing;
        }

        return terraPosition;
    }

    public void SetTerraAtPosition(TerraBattlePosition battlePosition)
    {
        if(!terraObjectByPosition.ContainsKey(battlePosition)) {
            Debug.LogWarning("The provided terra battle position was not found in BattleStage");
            return;
        }

        Vector3 terraPos = (Vector3)GetTerraPosition(battlePosition);
        Vector3 terraRot = Vector3.zero;
        terraRot.y = battlePosition.IsPrimarySide() ? battlefieldOrigin.eulerAngles.y : battlefieldOrigin.eulerAngles.y - 180f;

        if (terraObjectByPosition[battlePosition] != null)
            Destroy(terraObjectByPosition[battlePosition]);

        if(battlePosition.GetTerra() != null) {
            terraObjectByPosition[battlePosition] = Instantiate(battlePosition.GetTerra().GetTerraBase().GetTerraGameObject());
            terraObjectByPosition[battlePosition].transform.position = terraPos;
            terraObjectByPosition[battlePosition].transform.eulerAngles = terraRot;
        }
    }
}
