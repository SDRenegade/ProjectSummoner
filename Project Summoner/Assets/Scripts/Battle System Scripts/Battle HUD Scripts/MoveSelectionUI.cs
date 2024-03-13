using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelectionUI : MonoBehaviour
{
    public static readonly string EMPTY_SLOT_NAME = "--  --";

    [SerializeField] private Button[] moveBtns;
    [SerializeField] private Button exitBtn;

    public void OpenMoveSelectionUI(List<TerraMove> terraMoves, List<int> disabledMoves)
    {
        for (int i = 0; i < Terra.MOVE_SLOTS; i++) {
            if (moveBtns.Length <= i)
                break;

            string moveBtnName = EMPTY_SLOT_NAME;
            if (i < terraMoves.Count && terraMoves[i] != null)
                moveBtnName = terraMoves[i] + " " + terraMoves[i].GetCurrentPP() + "/" + terraMoves[i].GetMaxPP();
            moveBtns[i].GetComponentInChildren<TextMeshProUGUI>().SetText(moveBtnName);

            if (i >= terraMoves.Count || terraMoves[i] == null || disabledMoves.Contains(i))
                moveBtns[i].interactable = false;
            else
                moveBtns[i].interactable = true;
        }
        gameObject.SetActive(true);
    }

}
