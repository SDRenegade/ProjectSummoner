using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerDieSlider : MonoBehaviour
{
    [SerializeField] private GameObject summonerDieSlotPrefab;
    [SerializeField] private int numDicePreview;
    [SerializeField] private float slotSpacing;
    private List<SummonerDieSlotUI> summonerDieSlotList;

    private int selectedSlotIndex;

    public void Start()
    {
        selectedSlotIndex = 0;

        summonerDieSlotList = new List<SummonerDieSlotUI>();
        for (int i = 0; i < numDicePreview * 2 + 1; i++) {
            SummonerDieSlotUI summonerDieSlotUI = Instantiate(summonerDieSlotPrefab).GetComponent<SummonerDieSlotUI>();
            summonerDieSlotUI.transform.SetParent(transform);
            summonerDieSlotList.Add(summonerDieSlotUI);
        }
        InitSlotPositionsAndClickEvents();
    }

    //TODO Add click events to the buttons so they update the selectedSlotIndex
    public void InitSlotPositionsAndClickEvents()
    {
        for (int i = 0; i < summonerDieSlotList.Count; i++) {
            summonerDieSlotList[i].transform.position = transform.position;
            if (i <= numDicePreview)
                summonerDieSlotList[i].transform.Translate(new Vector3(i * slotSpacing, 0, 0));
            else {
                float spacing = (summonerDieSlotList.Count - i) * -slotSpacing;
                summonerDieSlotList[i].transform.Translate(new Vector3(spacing, 0, 0));
            }
        }
    }

    public void UpdateSummonerDieSlider(List<SummonerDieBase> summonerDieList)
    {
        int numRightSideDicePreview = summonerDieList.Count / 2;
        int numLeftSideDicePreview = summonerDieList.Count - 1 / 2;


    }

}
