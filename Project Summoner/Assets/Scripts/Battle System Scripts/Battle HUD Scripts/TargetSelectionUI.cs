using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUI : MonoBehaviour
{
    [SerializeField] private Button opponentPos1Btn;
    [SerializeField] private Button opponentPos2Btn;
    [SerializeField] private Button allyPosBtn;

    public void OpenTargetSelectionUI(TerraBattlePosition terraBattlePosition, Battlefield battlefield)
    {
        TerraBattlePosition[] primaryTerraBattlePositions = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositions = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition allyPosition = null;
        for (int i = 0; i < primaryTerraBattlePositions.Length; i++) {
            if (primaryTerraBattlePositions[i] == terraBattlePosition)
                continue;
            allyPosition = primaryTerraBattlePositions[i];
            break;
        }

        if (secondaryTerraBattlePositions[0].GetTerra() != null) {
            opponentPos1Btn.gameObject.SetActive(true);
            opponentPos1Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(secondaryTerraBattlePositions[0].GetTerra().ToString());
        }
        else
            opponentPos1Btn.gameObject.SetActive(false);
        if (secondaryTerraBattlePositions[1].GetTerra() != null) {
            opponentPos2Btn.gameObject.SetActive(true);
            opponentPos2Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(secondaryTerraBattlePositions[1].GetTerra().ToString());
        }
        else
            opponentPos2Btn.gameObject.SetActive(false);
        if (allyPosition != null) {
            allyPosBtn.gameObject.SetActive(true);
            allyPosBtn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(allyPosition.GetTerra().ToString());
        }
        else
            allyPosBtn.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
