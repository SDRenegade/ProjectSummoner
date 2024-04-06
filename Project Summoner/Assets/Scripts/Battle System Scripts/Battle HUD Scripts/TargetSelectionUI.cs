using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUI : MonoBehaviour
{
    [SerializeField] private Button opponent1Btn;
    [SerializeField] private Button opponent2Btn;
    [SerializeField] private Button ally1Btn;
    [SerializeField] private Button ally2Btn;

    public void OpenTargetSelectionUI(TerraBattlePosition[] targetableOpponentPositions, TerraBattlePosition[] targetableAllyPositions)
    {
        SetAllButtonActives(false);

        if (targetableOpponentPositions.Length >= 1 && targetableOpponentPositions[0] != null && targetableOpponentPositions[0].GetTerra() != null) {
            opponent1Btn.gameObject.SetActive(true);
            opponent1Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(targetableOpponentPositions[0].GetTerra().ToString());
        }
        if (targetableOpponentPositions.Length >= 2 && targetableOpponentPositions[1] != null && targetableOpponentPositions[1].GetTerra() != null) {
            opponent2Btn.gameObject.SetActive(true);
            opponent2Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(targetableOpponentPositions[1].GetTerra().ToString());
        }
        if (targetableAllyPositions.Length >= 1 && targetableAllyPositions[0] != null && targetableAllyPositions[0].GetTerra() != null) {
            ally1Btn.gameObject.SetActive(true);
            ally1Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(targetableAllyPositions[0].GetTerra().ToString());
        }
        if (targetableAllyPositions.Length >= 2 && targetableAllyPositions[1] != null && targetableAllyPositions[1].GetTerra() != null) {
            ally2Btn.gameObject.SetActive(true);
            ally2Btn.GetComponentInChildren<TextMeshProUGUI>()?.SetText(targetableAllyPositions[1].GetTerra().ToString());
        }

        gameObject.SetActive(true);
    }

    private void SetAllButtonActives(bool isActive)
    {
        opponent1Btn.gameObject.SetActive(isActive);
        opponent2Btn.gameObject.SetActive(isActive);
        ally1Btn.gameObject.SetActive(isActive);
        ally2Btn.gameObject.SetActive(isActive);
    }

    public Button GetOpponenet1Btn() { return opponent1Btn; }

    public Button GetOpponenet2Btn() { return opponent2Btn; }

    public Button GetAlly1Btn() { return ally1Btn; }

    public Button GetAlly2Btn() { return ally2Btn; }
}
