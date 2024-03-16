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

    //TODO Figure out how to not use magic numbers for number of banners
    public void Start()
    {
        BattleFormat battleFormat = BattleLoader.GetInstance().GetBattleFormat();
        leadingBannerList = (battleFormat == BattleFormat.SINGLE) ? new GameObject[1] : new GameObject[2];
        benchBannerList = (battleFormat == BattleFormat.SINGLE) ? new GameObject[5] : new GameObject[4];

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
