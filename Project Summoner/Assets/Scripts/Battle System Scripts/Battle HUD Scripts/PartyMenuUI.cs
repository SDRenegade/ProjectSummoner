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
    [Header("Option Selection Menu")]
    [SerializeField] private PartyOptionSelectionUI optionSelectionUI;

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

    public void OpenPartyMenuUI(List<Terra> terraList, BattleSystem battleSystem)
    {
        for (int i = 0; i < leadingBannerList.Length; i++) {
            if (terraList.Count <= i) {
                leadingBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(null, null, optionSelectionUI, battleSystem);
                continue;
            }

            leadingBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(terraList[i], i, optionSelectionUI, battleSystem);
        }

        for (int i = 0; i < benchBannerList.Length; i++) {
            if (terraList.Count <= i + leadingBannerList.Length) {
                benchBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(null, null, optionSelectionUI, battleSystem);
                continue;
            }

            benchBannerList[i].GetComponent<PartyBanner>().UpdatePartyBanner(terraList[i + leadingBannerList.Length], i + leadingBannerList.Length, optionSelectionUI, battleSystem);
        }
        
        gameObject.SetActive(true);
    }

    public void ClosePartyMenuUI()
    {
        optionSelectionUI.CloseOptionSelection();
        gameObject.SetActive(false);
    }
}
