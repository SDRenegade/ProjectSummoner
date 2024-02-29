using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUI : MonoBehaviour
{
    [SerializeField] private Button opponentPos1Btn;
    [SerializeField] private Button opponentPos2Btn;
    [SerializeField] private Button allyPos1Btn;
    [SerializeField] private Button allyPos2Btn;

    public void OpenTargetSelectionUI(TerraBattlePosition terraBattlePosition, Battlefield battlefield)
    {
        SetAllButtonActives(false);

        TerraBattlePosition[] primaryTerraBattlePositions = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositions = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();
        int? allyPositionIndex = null;
        for (int i = 0; i < primaryTerraBattlePositions.Length; i++) {
            if (primaryTerraBattlePositions[i] == terraBattlePosition)
                continue;
            allyPositionIndex = i;
            break;
        }

        if (secondaryTerraBattlePositions[0].GetTerra() != null) {
            opponentPos1Btn.gameObject.SetActive(true);
            opponentPos1Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(secondaryTerraBattlePositions[0].GetTerra().ToString());
        }
        if (secondaryTerraBattlePositions[1].GetTerra() != null) {
            opponentPos2Btn.gameObject.SetActive(true);
            opponentPos2Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(secondaryTerraBattlePositions[1].GetTerra().ToString());
        }
        if (allyPositionIndex != null) {
            if(allyPositionIndex == 0) {
                allyPos1Btn.gameObject.SetActive(true);
                allyPos1Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(primaryTerraBattlePositions[0].GetTerra().ToString());
            }
            else if(allyPositionIndex == 1) {
                allyPos2Btn.gameObject.SetActive(true);
                allyPos2Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(primaryTerraBattlePositions[1].GetTerra().ToString());
            }
        }
        else
            allyPos1Btn.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    private void SetAllButtonActives(bool isActive)
    {
        opponentPos1Btn.gameObject.SetActive(isActive);
        opponentPos2Btn.gameObject.SetActive(isActive);
        allyPos1Btn.gameObject.SetActive(isActive);
        allyPos2Btn.gameObject.SetActive(isActive);
    }
}
