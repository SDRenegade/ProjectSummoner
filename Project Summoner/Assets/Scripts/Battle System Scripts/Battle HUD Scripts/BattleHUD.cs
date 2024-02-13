using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> primarySideTerraStatusBarList;
    [SerializeField] private List<GameObject> secondarySideTerraStatusBarList;
    [SerializeField] private GameObject menuSelectionObject;
    [SerializeField] private GameObject moveSelectionObject;

    public void Start()
    {
        for(int i = 0; i < primarySideTerraStatusBarList.Count; i++)
            primarySideTerraStatusBarList[i].SetActive(false);
        for (int i = 0; i < secondarySideTerraStatusBarList.Count; i++)
            secondarySideTerraStatusBarList[i].SetActive(false);

        CloseAllSelectionUI();
    }

    public void UpdateTerraStatusBars(Battlefield battlefield, BattleFormat battleFormat)
    {
        TerraBattlePosition[] primaryTerraBattlePositionArr = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositionArr = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();

        int numStatusBars = (battleFormat == BattleFormat.SINGLE) ? 1 : 2;
        for(int i = 0; i < numStatusBars; i++) {
            if (primaryTerraBattlePositionArr[i] == null)
                primarySideTerraStatusBarList[i].SetActive(false);
            else {
                primarySideTerraStatusBarList[i].SetActive(true);
                primarySideTerraStatusBarList[i].GetComponent<TerraBattleStatusBar>()?.UpdateStatusBar(primaryTerraBattlePositionArr[i].GetTerra());
            }

            if (primaryTerraBattlePositionArr[i] == null)
                secondarySideTerraStatusBarList[i].SetActive(false);
            else {
                secondarySideTerraStatusBarList[i].SetActive(true);
                secondarySideTerraStatusBarList[i].GetComponent<TerraBattleStatusBar>()?.UpdateStatusBar(secondaryTerraBattlePositionArr[i].GetTerra());
            }
        }
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
