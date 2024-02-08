using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TerraBattleStatusBar playerTerraStatusBar;
    [SerializeField] private TerraBattleStatusBar opponentTerraStatusBar;
    [SerializeField] private GameObject menuSelectionObject;
    [SerializeField] private GameObject moveSelectionObject;

    public void Start()
    {
        CloseAllSelectionUI();
    }

    public void UpdateTerraStatusBars(Battlefield battlefield)
    {
        playerTerraStatusBar.UpdateStatusBar(battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0].GetTerra());
        opponentTerraStatusBar.UpdateStatusBar(battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0].GetTerra());
    }

    public void OpenMenuSelectionUI()
    {
        moveSelectionObject.SetActive(false);
        menuSelectionObject.SetActive(true);
    }

    public void OpenMoveSelectionUI(List<TerraMove> terraMoves, List<int> disabledMoves)
    {
        menuSelectionObject.SetActive(false);

        MoveSelectionUI moveSelectionUI = moveSelectionObject.GetComponent<MoveSelectionUI>();
        for (int i = 0; i < Terra.MOVE_SLOTS; i++) {
            if (moveSelectionUI.GetMoveBtns().Length <= i)
                break;

            string moveBtnName = MoveSelectionUI.EMPTY_SLOT_NAME;
            if (i < terraMoves.Count && terraMoves[i] != null)
                moveBtnName = terraMoves[i] + " " + terraMoves[i].GetCurrentPP() + "/" + terraMoves[i].GetMaxPP();
            moveSelectionUI.GetMoveBtns()[i].GetComponentInChildren<TextMeshProUGUI>().SetText(moveBtnName);

            if(i >= terraMoves.Count || terraMoves[i] == null || disabledMoves.Contains(i))
                moveSelectionUI.GetMoveBtns()[i].interactable = false;
            else
                moveSelectionUI.GetMoveBtns()[i].interactable = true;
        }
        moveSelectionObject.SetActive(true);
    }

    public void CloseAllSelectionUI()
    {
        moveSelectionObject.SetActive(false);
        menuSelectionObject.SetActive(false);
    }
}
