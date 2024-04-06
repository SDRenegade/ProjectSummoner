using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO Move update logic to the SummonerDieSliderUI class
public class SummonerDieSlotUI : MonoBehaviour
{
    private readonly static float PREVIEW_DISTANCE_1_ALPHA = 0.95f;
    private readonly static float PREVIEW_DISTANCE_2_ALPHA = 0.75f;
    private readonly static float PREVIEW_DISTANCE_3_ALPHA = 0.5f;
    private readonly static Color32 SELECTED_DIE_QUANTITY_TEXT_COLOR = new Color32(0, 0, 0, 255);
    private readonly static Color32 PREVIEW_DIE_QUANTITY_TEXT_COLOR = new Color32(255, 255, 255, 255);

    [SerializeField] private Image summonerDieImage;
    [SerializeField] private Image dieQuantityPanel;
    [SerializeField] private TextMeshProUGUI dieQuantityText;

    private float originalImageAlpha;
    private float originalPanelAlpha;
    private float originalTextAlpha;

    public void Awake()
    {
        originalImageAlpha = summonerDieImage.color.a;
        originalPanelAlpha = dieQuantityPanel.color.a;
        originalTextAlpha = dieQuantityText.color.a;
    }

    public void UpdateSummonerDiePreview(SummonerDieItemStack summonerDieItemStack, int previewDistance)
    {
        if(previewDistance == 0) {
            dieQuantityPanel.gameObject.SetActive(false);
            dieQuantityText.color = SELECTED_DIE_QUANTITY_TEXT_COLOR;
        }
        else {
            dieQuantityPanel.gameObject.SetActive(true);
            dieQuantityText.color = PREVIEW_DIE_QUANTITY_TEXT_COLOR;
        }
        summonerDieImage.sprite = summonerDieItemStack.GetSummonerDieBase().GetItemSO().GetSprite();
        dieQuantityText.SetText(summonerDieItemStack.GetAmount().ToString());
        UpdateSlotAlphaFromPreview(previewDistance);
    }

    private void UpdateSlotAlphaFromPreview(int previewDistance)
    {
        float alpha = 1f;
        if(previewDistance == 1)
            alpha = PREVIEW_DISTANCE_1_ALPHA;
        else if(previewDistance == 2)
            alpha = PREVIEW_DISTANCE_2_ALPHA;
        else if(previewDistance >= 3)
            alpha = PREVIEW_DISTANCE_3_ALPHA;

        summonerDieImage.color = new Color(summonerDieImage.color.r, summonerDieImage.color.g, summonerDieImage.color.b, originalImageAlpha * alpha);
        dieQuantityPanel.color = new Color(dieQuantityPanel.color.r, dieQuantityPanel.color.g, dieQuantityPanel.color.b, originalPanelAlpha * alpha);
        dieQuantityText.color = new Color(dieQuantityText.color.r, dieQuantityText.color.g, dieQuantityText.color.b, originalTextAlpha * alpha);
    }

    public Image GetSummonerDieImage() { return summonerDieImage; }

    public void SetSummonerDieImage(Image summonerDieImage) { this.summonerDieImage = summonerDieImage; }

    public Image GetDieQuantityPanel() { return dieQuantityPanel; }

    public void SetDieQuantityPanel(Image dieQuantityPanel) { this.dieQuantityPanel = dieQuantityPanel; }

    public TextMeshProUGUI GetDieQuanityText() { return dieQuantityText; }

    public void SetDieQuanityText(TextMeshProUGUI dieQuantityText) {  this.dieQuantityText = dieQuantityText; }
}
