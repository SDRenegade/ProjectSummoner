using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonerDieSlotUI : MonoBehaviour
{
    [SerializeField] private Image summonerDieImage;
    [SerializeField] private RectTransform dieQuanityPanel;
    [SerializeField] private TextMeshProUGUI dieQuantityText;

    public Image GetSummonerDieImage() { return summonerDieImage; }

    public void SetSummonerDieImage(Image summonerDieImage) { this.summonerDieImage = summonerDieImage; }

    public RectTransform GetDieQuantityPanel() { return dieQuanityPanel; }

    public void SetDieQuantityPanel(RectTransform dieQuanityPanel) { this.dieQuanityPanel = dieQuanityPanel; }

    public TextMeshProUGUI GetDieQuanityText() { return dieQuantityText; }

    public void SetDieQuanityText(TextMeshProUGUI dieQuantityText) {  this.dieQuantityText = dieQuantityText; }
}
