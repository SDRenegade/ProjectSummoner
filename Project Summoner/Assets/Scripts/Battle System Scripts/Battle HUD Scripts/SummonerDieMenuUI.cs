using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerDieMenuUI : MonoBehaviour
{
    [SerializeField] private SummonerDieSlider summonerDieSlider;

    public void OpenSummonerDieMenuUI(List<SummonerDieItemStack> summonerDieItemStackList)
    {
        summonerDieSlider.UpdateSummonerDieSlider(summonerDieItemStackList);
        gameObject.SetActive(true);
    }

    public void CloseSummonerDieMenuUI()
    {
        gameObject.SetActive(false);
    }

}
