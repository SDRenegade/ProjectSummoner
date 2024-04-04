using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SummonerDieSlider : MonoBehaviour
{
    [SerializeField] private GameObject summonerDieSlotPrefab;
    [SerializeField] private int maxDicePreview;
    [SerializeField] private float slotSpacing;
    private List<SummonerDieSlotUI> summonerDieSlotList;

    private int selectedSlotIndex;

    public void Start()
    {
        selectedSlotIndex = 0;

        summonerDieSlotList = new List<SummonerDieSlotUI>();
        for (int i = 0; i < maxDicePreview * 2 + 1; i++) {
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
            if (i <= maxDicePreview)
                summonerDieSlotList[i].transform.Translate(new Vector3(i * slotSpacing, 0, 0));
            else {
                float spacing = (i - maxDicePreview) * -slotSpacing;
                summonerDieSlotList[i].transform.Translate(new Vector3(spacing, 0, 0));
            }
        }
    }

    public void UpdateSummonerDieSlider(List<SummonerDieItemStack> summonerDieItemStackList)
    {
        int numRightSideDicePreview = (summonerDieItemStackList.Count >= maxDicePreview * 2) ? maxDicePreview : summonerDieItemStackList.Count / 2;
        int numLeftSideDicePreview = (summonerDieItemStackList.Count >= maxDicePreview * 2 + 1) ? maxDicePreview : (summonerDieItemStackList.Count - 1) / 2;
        Debug.Log("Right preview: " + numRightSideDicePreview + " Left preview: " + numLeftSideDicePreview);

        for (int i = 0; i <= maxDicePreview; i++) {
            if (i >= summonerDieItemStackList.Count || i > numRightSideDicePreview) {
                summonerDieSlotList[i].gameObject.SetActive(false);
                continue;
            }
            summonerDieSlotList[i].UpdateSummonerDiePreview(summonerDieItemStackList[i], i);
            summonerDieSlotList[i].gameObject.SetActive(true);
        }
        for(int i = 0; i < maxDicePreview; i++) {
            if (numRightSideDicePreview + i + 1 >= summonerDieItemStackList.Count || i >= numLeftSideDicePreview) {
                summonerDieSlotList[maxDicePreview + i + 1].gameObject.SetActive(false);
                continue;
            }
            summonerDieSlotList[maxDicePreview + i + 1].UpdateSummonerDiePreview(summonerDieItemStackList[summonerDieItemStackList.Count - i - 1], i + 1);
            summonerDieSlotList[maxDicePreview + i + 1].gameObject.SetActive(true);
        }
    }

}
