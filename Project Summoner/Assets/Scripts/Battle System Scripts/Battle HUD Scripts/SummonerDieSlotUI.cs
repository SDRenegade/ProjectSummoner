using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonerDieSlotUI : MonoBehaviour
{
    [SerializeField] private Image summonerDieImage;
    [SerializeField] private Image dieQuanityPanel;
    [SerializeField] private TextMeshProUGUI dieQuantityText;

    private float originalImageAlpha;
    private float originalPanelAlpha;
    private float originalTextAlpha;

    public void Awake()
    {
        originalImageAlpha = summonerDieImage.color.a;
        originalPanelAlpha = dieQuanityPanel.color.a;
        originalTextAlpha = dieQuantityText.color.a;
    }

    public void UpdateSummonerDiePreview(SummonerDieItemStack summonerDieItemStack, int previewDistance)
    {
        summonerDieImage.sprite = summonerDieItemStack.GetSummonerDieBase().GetItemSO().GetSprite();
        dieQuantityText.SetText(summonerDieItemStack.GetAmount().ToString());
        UpdateSlotAlphaFromPreview(previewDistance);
    }

    private void UpdateSlotAlphaFromPreview(int previewDistance)
    {
        float alpha = 1f;
        if(previewDistance == 1)
            alpha = 0.95f;
        else if(previewDistance == 2)
            alpha = 0.75f;
        else if(previewDistance >= 3)
            alpha = 0.5f;

        summonerDieImage.color = new Color(summonerDieImage.color.r, summonerDieImage.color.g, summonerDieImage.color.b, originalImageAlpha * alpha);
        dieQuanityPanel.color = new Color(dieQuanityPanel.color.r, dieQuanityPanel.color.g, dieQuanityPanel.color.b, originalPanelAlpha * alpha);
        dieQuantityText.color = new Color(dieQuantityText.color.r, dieQuantityText.color.g, dieQuantityText.color.b, originalTextAlpha * alpha);
    }

    public Image GetSummonerDieImage() { return summonerDieImage; }

    public void SetSummonerDieImage(Image summonerDieImage) { this.summonerDieImage = summonerDieImage; }

    public Image GetDieQuantityPanel() { return dieQuanityPanel; }

    public void SetDieQuantityPanel(Image dieQuanityPanel) { this.dieQuanityPanel = dieQuanityPanel; }

    public TextMeshProUGUI GetDieQuanityText() { return dieQuantityText; }

    public void SetDieQuanityText(TextMeshProUGUI dieQuantityText) {  this.dieQuantityText = dieQuantityText; }
}
