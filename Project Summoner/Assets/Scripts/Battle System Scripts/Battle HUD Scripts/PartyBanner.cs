using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyBanner : MonoBehaviour
{
    private static readonly Color32 HIGH_HP_RANGE_COLOR = new Color32(20, 200, 0, 255);
    private static readonly Color32 MEDIUM_HP_RANGE_COLOR = new Color32(240, 250, 0, 255);
    private static readonly Color32 LOW_HP_RANGE_COLOR = new Color32(210, 0, 0, 255);
    private static readonly Color32 EMPTY_BANNER_IMAGE_COLOR = new Color32(0, 0, 0, 255);
    private static readonly Color32 NORMAL_BANNER_COLOR = new Color32(225, 225, 225, 255);
    private static readonly Color32 FAINTED_BANNER_COLOR = new Color32(185, 85, 60, 255);

    [SerializeField] private GameObject detailPanelLayer;
    [SerializeField] private Image image;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI terraName;
    [SerializeField] private TextMeshProUGUI terraLevel;
    [SerializeField] private TextMeshProUGUI terraCurrentHealth;
    [SerializeField] private TextMeshProUGUI terraMaxHealth;

    public void UpdatePartyBanner(TerraBattlePosition activeTerraPosition, int? terraPartyIndex, PartyOptionSelectionUI optionSelectionUI, Action<TerraBattlePosition, TerraSwitch> switchAction, BattleSystem battleSystem)
    {
        if (terraPartyIndex == null || battleSystem.GetPrimaryTerraList().Count <= terraPartyIndex || battleSystem.GetPrimaryTerraList()[(int)terraPartyIndex] == null) {
            SetEmptyBanner();
            return;
        }

        List<Terra> terraList = activeTerraPosition.GetBattleSide().IsPrimarySide() ? battleSystem.GetPrimaryTerraList() : battleSystem.GetSecondaryTerraList();
        Terra terra = terraList[(int)terraPartyIndex];

        //TODO Set Image to sprite of terra

        terraName.SetText(terra.GetTerraBase().GetSpeciesName());
        terraLevel.SetText(terra.GetLevel().ToString());

        Image bannerImage = GetComponent<Image>();
        if (terra.GetCurrentHP() == 0)
            bannerImage.color = FAINTED_BANNER_COLOR;
        else
            bannerImage.color = NORMAL_BANNER_COLOR;

        float progressValue = (float)terra.GetCurrentHP() / terra.GetMaxHP();
        healthBar.SetProgress(progressValue);
        if (progressValue > 0.5f)
            healthBar.GetImage().color = HIGH_HP_RANGE_COLOR;
        else if (progressValue > 0.25)
            healthBar.GetImage().color = MEDIUM_HP_RANGE_COLOR;
        else
            healthBar.GetImage().color = LOW_HP_RANGE_COLOR;

        terraCurrentHealth.SetText(terra.GetCurrentHP().ToString());
        terraMaxHealth.SetText(terra.GetMaxHP().ToString());

        Button partyBannerBtn = GetComponent<Button>();
        partyBannerBtn.onClick.AddListener(delegate {
            optionSelectionUI.OpenOptionSelctionUI(activeTerraPosition, (int)terraPartyIndex, switchAction, battleSystem);
        });
    }

    private void SetEmptyBanner()
    {
        image.sprite = null;
        image.color = EMPTY_BANNER_IMAGE_COLOR;

        detailPanelLayer.gameObject.SetActive(false);
        Button partyBannerBtn = GetComponent<Button>();
        partyBannerBtn.interactable = false;
    }
}
