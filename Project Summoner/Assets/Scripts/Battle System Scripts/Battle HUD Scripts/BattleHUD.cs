using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private List<TerraBattleStatusBar> primarySideTerraStatusBarList;
    [SerializeField] private List<TerraBattleStatusBar> secondarySideTerraStatusBarList;
    [SerializeField] private MenuSelectionUI menuSelectionUI;
    [SerializeField] private MoveSelectionUI moveSelectionUI;
    [SerializeField] private TargetSelectionUI targetSelectionUI;
    [SerializeField] private PartyMenuUI partyMenuUI;

    public void Start()
    {
        for(int i = 0; i < primarySideTerraStatusBarList.Count; i++)
            primarySideTerraStatusBarList[i].gameObject.SetActive(false);
        for (int i = 0; i < secondarySideTerraStatusBarList.Count; i++)
            secondarySideTerraStatusBarList[i].gameObject.SetActive(false);

        CloseAllSelectionUI();
    }

    public void UpdateTerraStatusBars(Battlefield battlefield, BattleFormat battleFormat)
    {
        TerraBattlePosition[] primaryTerraBattlePositionArr = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositionArr = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();

        int numStatusBars = battleFormat.NumberOfLeadingPositions();
        for(int i = 0; i < numStatusBars; i++) {
            if (primaryTerraBattlePositionArr[i].GetTerra() == null)
                primarySideTerraStatusBarList[i].gameObject.SetActive(false);
            else {
                primarySideTerraStatusBarList[i].gameObject.SetActive(true);
                primarySideTerraStatusBarList[i].UpdateStatusBar(primaryTerraBattlePositionArr[i].GetTerra());
            }

            if (secondaryTerraBattlePositionArr[i].GetTerra() == null)
                secondarySideTerraStatusBarList[i].gameObject.SetActive(false);
            else {
                secondarySideTerraStatusBarList[i].gameObject.SetActive(true);
                secondarySideTerraStatusBarList[i].UpdateStatusBar(secondaryTerraBattlePositionArr[i].GetTerra());
            }
        }
    }

    public void OpenMenuSelectionUI(BattleActionManager battleActionManager)
    {
        CloseAllSelectionUI();
        menuSelectionUI.OpenMenuSelectionUI(battleActionManager);
    }

    public void ExitMenuSelection(BattleActionManager battleActionManger)
    {
        menuSelectionUI.ExitMenuSelection(battleActionManger);
    }

    public void OpenMoveSelectionUI(List<TerraMove> terraMoves, List<int> disabledMoves)
    {
        CloseAllSelectionUI();
        moveSelectionUI.OpenMoveSelectionUI(terraMoves, disabledMoves);
    }

    public void OpenTargetSelectionUI(TerraBattlePosition terraBattlePosition, Battlefield battlefield)
    {
        CloseAllSelectionUI();
        targetSelectionUI.OpenTargetSelectionUI(terraBattlePosition, battlefield);
    }

    public void OpenPartyMenuUI(TerraBattlePosition activeTerraPosition, List<Terra> terraList, bool isMustSwitch, Action<TerraBattlePosition, TerraSwitch> switchAction, BattleSystem battleSystem)
    {
        CloseAllSelectionUI();
        HideTerraStatusBars();
        partyMenuUI.OpenPartyMenuUI(activeTerraPosition, terraList, isMustSwitch, switchAction, battleSystem);
    }

    public void ExitPartyMenuUI(Battlefield battlefield, BattleFormat battleFormat, BattleActionManager battleActionManager)
    {
        UpdateTerraStatusBars(battlefield, battleFormat);
        OpenMenuSelectionUI(battleActionManager);
    }

    public void HideTerraStatusBars()
    {
        for(int i = 0; i < primarySideTerraStatusBarList.Count; i++)
            primarySideTerraStatusBarList[i].gameObject.SetActive(false);
        for(int i = 0; i < secondarySideTerraStatusBarList.Count; i++)
            secondarySideTerraStatusBarList[i].gameObject.SetActive(false);
    }

    public void CloseAllSelectionUI()
    {
        menuSelectionUI.gameObject.SetActive(false);
        moveSelectionUI.gameObject.SetActive(false);
        targetSelectionUI.gameObject.SetActive(false);
        partyMenuUI.ClosePartyMenuUI();
    }
}
