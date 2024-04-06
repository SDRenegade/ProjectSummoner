using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SummonerDieSlider : MonoBehaviour
{
    [SerializeField] private GameObject summonerDieSlotPrefab;
    [SerializeField] private TextMeshProUGUI dieTitleText;
    [SerializeField] private Button backIterationBtn;
    [SerializeField] private Button forwardIterationBtn;
    [SerializeField] private int maxDicePreview;
    [SerializeField] private float selectedSlotSpacing;
    [SerializeField] private float previewSlotSpacing;

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
        InitSlotPositions();
    }

    private void InitSlotPositions()
    {
        for (int i = 0; i < summonerDieSlotList.Count; i++) {
            summonerDieSlotList[i].transform.position = transform.position;
            if (i == 0)
                continue;
            
            if (i <= maxDicePreview)
                summonerDieSlotList[i].transform.Translate(new Vector3((i - 1) * previewSlotSpacing + selectedSlotSpacing, 0, 0));
            else {
                float spacing = (i - maxDicePreview - 1) * -previewSlotSpacing - selectedSlotSpacing;
                summonerDieSlotList[i].transform.Translate(new Vector3(spacing, 0, 0));
            }
        }
    }

    public void UpdateSummonerDieSlider(List<SummonerDieItemStack> summonerDieItemStackList, int offset)
    {
        OffsetSelectedSlotIndex(summonerDieItemStackList.Count, offset);

        dieTitleText.SetText(summonerDieItemStackList[selectedSlotIndex].GetSummonerDieBase().ToString());
        backIterationBtn.onClick.RemoveAllListeners();
        backIterationBtn.onClick.AddListener(() => {
            UpdateSummonerDieSlider(summonerDieItemStackList, -1);
        });
        forwardIterationBtn.onClick.RemoveAllListeners();
        forwardIterationBtn.onClick.AddListener(() => {
            UpdateSummonerDieSlider(summonerDieItemStackList, 1);
        });

        int numRightSideDicePreview = (summonerDieItemStackList.Count >= maxDicePreview * 2) ? maxDicePreview : summonerDieItemStackList.Count / 2;
        int numLeftSideDicePreview = (summonerDieItemStackList.Count >= maxDicePreview * 2 + 1) ? maxDicePreview : (summonerDieItemStackList.Count - 1) / 2;

        for (int i = 0; i <= maxDicePreview; i++) {
            if (i >= summonerDieItemStackList.Count || i > numRightSideDicePreview) {
                summonerDieSlotList[i].gameObject.SetActive(false);
                continue;
            }

            int offsetIndex = i + selectedSlotIndex;
            if (offsetIndex >= summonerDieItemStackList.Count)
                offsetIndex = offsetIndex % summonerDieItemStackList.Count;
            summonerDieSlotList[i].UpdateSummonerDiePreview(summonerDieItemStackList[offsetIndex], i);
            summonerDieSlotList[i].gameObject.SetActive(true);
        }
        for(int i = 0; i < maxDicePreview; i++) {
            if (numRightSideDicePreview + i + 1 >= summonerDieItemStackList.Count || i >= numLeftSideDicePreview) {
                summonerDieSlotList[maxDicePreview + i + 1].gameObject.SetActive(false);
                continue;
            }

            int offsetIndex = selectedSlotIndex - i - 1;
            if (offsetIndex < 0)
                offsetIndex = summonerDieItemStackList.Count + offsetIndex;
            summonerDieSlotList[maxDicePreview + i + 1].UpdateSummonerDiePreview(summonerDieItemStackList[offsetIndex], i + 1);
            summonerDieSlotList[maxDicePreview + i + 1].gameObject.SetActive(true);
        }
    }

    public void OffsetSelectedSlotIndex(int listLength, int offset)
    {
        int modOffset = (offset > 0) ? offset % listLength : offset % -listLength;
        int tempSelectionIndex = selectedSlotIndex + modOffset;
        if (tempSelectionIndex >= listLength)
            selectedSlotIndex = tempSelectionIndex % listLength;
        else if (tempSelectionIndex < 0)
            selectedSlotIndex = listLength + tempSelectionIndex;
        else
            selectedSlotIndex = tempSelectionIndex;
    }

}
