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

    public void UpdateTerraStatusBars(Battlefield battlefield)
    {
        playerTerraStatusBar.UpdateStatusBar(battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra());
        opponentTerraStatusBar.UpdateStatusBar(battlefield.GetSecondaryBattleSide().GetTerraBattlePosition().GetTerra());
    }

    public void OpenMenuSelectionUI()
    {
        moveSelectionObject.SetActive(false);
        menuSelectionObject.SetActive(true);
    }

    public void OpenMoveSelectionUI(List<TerraMove> terraMoves)
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
        }
        moveSelectionObject.SetActive(true);
    }

    public void CloseAllSelectionUI()
    {
        moveSelectionObject.SetActive(false);
        menuSelectionObject.SetActive(false);
    }
}
