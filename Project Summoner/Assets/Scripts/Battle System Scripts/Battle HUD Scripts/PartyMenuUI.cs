using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject leadingBannerPrefab;
    [SerializeField] private GameObject benchBannerPrefab;
    [Header("Stack Panels")]
    [SerializeField] private GameObject leadingStackPanel;
    [SerializeField] private GameObject benchStackPanel;
    [Space]
    [SerializeField] private PartyOptionSelectionUI optionSelectionUI;
    [SerializeField] private Button cancelBtn;

    private GameObject[] leadingBannerList;
    private GameObject[] benchBannerList;

    public void Start()
    {
        BattleFormat battleFormat = BattleLoader.GetInstance().GetBattleFormat();
        leadingBannerList = new GameObject[battleFormat.NumberOfLeadingPositions()];
        benchBannerList = new GameObject[6 - battleFormat.NumberOfLeadingPositions()];

        InitPartyObjects();
        optionSelectionUI.gameObject.SetActive(false);
    }

    private void InitPartyObjects()
    {
        for(int i = 0; i < leadingBannerList.Length; i++) {
            leadingBannerList[i] = Instantiate(leadingBannerPrefab);
            leadingBannerList[i].transform.SetParent(leadingStackPanel.transform);
        }

        for(int i = 0; i < benchBannerList.Length; i++) {
            benchBannerList[i] = Instantiate(benchBannerPrefab);
            benchBannerList[i].transform.SetParent(benchStackPanel.transform);
        }
    }

    public void OpenPartyMenuUI(TerraBattlePosition activeTerraPosition, List<Terra> terraList, bool isMustSwitch, Action<TerraBattlePosition, TerraSwitch> switchAction, BattleSystem battleSystem)
    {
        for (int i = 0; i < leadingBannerList.Length; i++) {
            if (terraList.Count <= i) {
                leadingBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(activeTerraPosition, null, optionSelectionUI, switchAction, battleSystem);
                continue;
            }

            leadingBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(activeTerraPosition, i, optionSelectionUI, switchAction, battleSystem);
        }

        for (int i = 0; i < benchBannerList.Length; i++) {
            if (terraList.Count <= i + leadingBannerList.Length) {
                benchBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(activeTerraPosition, null, optionSelectionUI, switchAction, battleSystem);
                continue;
            }

            benchBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(activeTerraPosition, i + leadingBannerList.Length, optionSelectionUI, switchAction, battleSystem);
        }

        if (isMustSwitch)
            cancelBtn.interactable = false;
        else
            cancelBtn.interactable = true;

        gameObject.SetActive(true);
    }

    public void ClosePartyMenuUI()
    {
        optionSelectionUI.CloseOptionSelection();
        gameObject.SetActive(false);
    }
}
