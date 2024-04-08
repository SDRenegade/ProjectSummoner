using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerDieMenuUI : MonoBehaviour
{
    [SerializeField] private SummonerDieSlider summonerDieSlider;

    public void InitButtonEvents(BattleSystem battleSystem)
    {
        summonerDieSlider.GetSelectionBtn().onClick.AddListener(() => {
            battleSystem.SummonerDieSelection(summonerDieSlider.GetSelectedSlotIndex());
        });
    }

    public void OpenSummonerDieMenuUI(List<SummonerDieItemStack> summonerDieItemStackList, int offset)
    {
        summonerDieSlider.UpdateSummonerDieSlider(summonerDieItemStackList, offset);
        gameObject.SetActive(true);
    }

    public void CloseSummonerDieMenuUI()
    {
        gameObject.SetActive(false);
    }

}
