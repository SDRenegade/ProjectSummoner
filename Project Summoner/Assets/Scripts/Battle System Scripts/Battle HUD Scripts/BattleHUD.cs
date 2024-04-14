using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TerraBattleStatusBarGroupUI terraBattleStatusBarGroupUI;
    [SerializeField] private MenuSelectionUI menuSelectionUI;
    [SerializeField] private MoveSelectionUI moveSelectionUI;
    [SerializeField] private TargetSelectionUI targetSelectionUI;
    [SerializeField] private PartyMenuUI partyMenuUI;
    [SerializeField] private SummonerDieMenuUI summonerDieMenuUI;

    public void Start()
    {
        CloseAllSelectionUI();
    }

    public void InitBattleHUD(BattleSystem battleSystem)
    {
        terraBattleStatusBarGroupUI.InitStatusBars(battleSystem.GetBattleFormat());

        targetSelectionUI.GetOpponenet1Btn().onClick.AddListener(() => battleSystem.TargetSelection(0));
        targetSelectionUI.GetOpponenet2Btn().onClick.AddListener(() => battleSystem.TargetSelection(1));
        targetSelectionUI.GetAlly1Btn().onClick.AddListener(() => battleSystem.TargetSelection(2));
        targetSelectionUI.GetAlly2Btn().onClick.AddListener(() => battleSystem.TargetSelection(3));
        summonerDieMenuUI.InitButtonEvents(battleSystem);
    }

    public void UpdateTerraStatusBars(Battlefield battlefield)
    {
        terraBattleStatusBarGroupUI.UpdateTerraStatusBars(battlefield);
    }

    public void HideTerraStatusBars()
    {
        terraBattleStatusBarGroupUI.HideTerraStatusBars();
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

    public void OpenTargetSelectionUI(TerraBattlePosition[] opponentTerraPositionss, TerraBattlePosition[] allyTerraPositions)
    {
        CloseAllSelectionUI();
        targetSelectionUI.OpenTargetSelectionUI(opponentTerraPositionss, allyTerraPositions);
    }

    public void OpenPartyMenuUI(TerraBattlePosition activeTerraPosition, List<Terra> terraList, bool isMustSwitch, Action<TerraBattlePosition, TerraSwitch> switchAction, BattleSystem battleSystem)
    {
        CloseAllSelectionUI();
        HideTerraStatusBars();
        partyMenuUI.OpenPartyMenuUI(activeTerraPosition, terraList, isMustSwitch, switchAction, battleSystem);
    }


    public void OpenSummonerDieMenuUI(List<SummonerDieItemStack> summonerDieItemStackList)
    {
        CloseAllSelectionUI();
        HideTerraStatusBars();
        summonerDieMenuUI.OpenSummonerDieMenuUI(summonerDieItemStackList, 0);
    }

    public void ReturnToMenuSelection(Battlefield battlefield, BattleActionManager battleActionManager)
    {
        UpdateTerraStatusBars(battlefield);
        OpenMenuSelectionUI(battleActionManager);
    }

    public void CloseAllSelectionUI()
    {
        menuSelectionUI.gameObject.SetActive(false);
        moveSelectionUI.gameObject.SetActive(false);
        targetSelectionUI.gameObject.SetActive(false);
        partyMenuUI.ClosePartyMenuUI();
        summonerDieMenuUI.CloseSummonerDieMenuUI();
    }

    public SummonerDieMenuUI GetSummonerDieMenuUI() { return summonerDieMenuUI; }
}
