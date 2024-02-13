using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerraBattleStatusBar : MonoBehaviour
{
    private static readonly Color32 HIGH_HP_RANGE_COLOR = new Color32(20, 200, 0, 255);
    private static readonly Color32 MEDIUM_HP_RANGE_COLOR = new Color32(240, 250, 0, 255);
    private static readonly Color32 LOW_HP_RANGE_COLOR = new Color32(210, 0, 0, 255);

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI terraNameTMP;
    [SerializeField] private TextMeshProUGUI terraLevelTMP;
    [SerializeField] private TextMeshProUGUI terraCurrentHealthTMP;
    [SerializeField] private TextMeshProUGUI terraMaxHealthTMP;

    public void UpdateStatusBar(Terra terra)
    {
        terraNameTMP.SetText(terra.GetTerraBase().GetSpeciesName());
        terraLevelTMP.SetText("Lvl " + terra.GetLevel().ToString());

        float progressValue = (float)terra.GetCurrentHP() / terra.GetMaxHP();
        healthBar.SetProgress(progressValue);
        if (progressValue > 0.5f)
            healthBar.GetImage().color = HIGH_HP_RANGE_COLOR;
        else if (progressValue > 0.25)
            healthBar.GetImage().color = MEDIUM_HP_RANGE_COLOR;
        else
            healthBar.GetImage().color = LOW_HP_RANGE_COLOR;

        terraCurrentHealthTMP.SetText(terra.GetCurrentHP().ToString());
        terraMaxHealthTMP.SetText(terra.GetMaxHP().ToString());
    }

    public ProgressBar GetHealthBar() { return healthBar; }
    
    public TextMeshProUGUI GetTerraNameTMP() { return terraNameTMP; }

    public TextMeshProUGUI GetTerraLevelTMP() { return terraLevelTMP; }

    public TextMeshProUGUI GetTerraCurrentHealthTMP() { return terraCurrentHealthTMP; }

    public TextMeshProUGUI GetTerraMaxHealthTMP() { return terraMaxHealthTMP; }
}
