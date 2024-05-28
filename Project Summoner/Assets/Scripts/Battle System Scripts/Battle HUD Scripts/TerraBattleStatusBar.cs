using TMPro;
using UnityEngine;

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

    float increment = 0.4f;
    float maxHealth;
    float currentHealth;
    float previousHealth;
    float interpolationValue;

    private void Update()
    {
        if (interpolationValue >= 1f)
            return;

        interpolationValue += increment * Time.deltaTime;
        if(interpolationValue > currentHealth)
            interpolationValue = currentHealth;

        float interpolationHealth = Mathf.Lerp(previousHealth, currentHealth, interpolationValue);
        healthBar.SetProgress(interpolationHealth / maxHealth);
        if (healthBar.GetProgress() > 0.5f)
            healthBar.GetImage().color = HIGH_HP_RANGE_COLOR;
        else if (healthBar.GetProgress() > 0.25)
            healthBar.GetImage().color = MEDIUM_HP_RANGE_COLOR;
        else
            healthBar.GetImage().color = LOW_HP_RANGE_COLOR;

        terraCurrentHealthTMP.SetText(((int)interpolationHealth).ToString());
    }

    public void StaticUpdateStatusBar(Terra terra)
    {
        maxHealth = terra.GetMaxHP();
        currentHealth = terra.GetCurrentHP();
        previousHealth = currentHealth;
        interpolationValue = 1f;

        terraNameTMP.SetText(terra.GetTerraBase().GetSpeciesName());
        terraLevelTMP.SetText("Lvl " + terra.GetLevel().ToString());
        terraMaxHealthTMP.SetText(terra.GetMaxHP().ToString());
        terraCurrentHealthTMP.SetText(terra.GetCurrentHP().ToString());

        healthBar.SetProgress(currentHealth / maxHealth);
        if (healthBar.GetProgress() > 0.5f)
            healthBar.GetImage().color = HIGH_HP_RANGE_COLOR;
        else if (healthBar.GetProgress() > 0.25)
            healthBar.GetImage().color = MEDIUM_HP_RANGE_COLOR;
        else
            healthBar.GetImage().color = LOW_HP_RANGE_COLOR;
    }

    public void DynamicUpdateStatusBar(Terra terra)
    {
        terraNameTMP.SetText(terra.GetTerraBase().GetSpeciesName());
        terraLevelTMP.SetText("Lvl " + terra.GetLevel().ToString());

        maxHealth = terra.GetMaxHP();
        float newCurrentHealth = terra.GetCurrentHP();
        if(currentHealth != newCurrentHealth) {
            previousHealth = currentHealth;
            currentHealth = newCurrentHealth;
            interpolationValue = 0f;
        }

        terraMaxHealthTMP.SetText(terra.GetMaxHP().ToString());
    }

    public ProgressBar GetHealthBar() { return healthBar; }
    
    public TextMeshProUGUI GetTerraNameTMP() { return terraNameTMP; }

    public TextMeshProUGUI GetTerraLevelTMP() { return terraLevelTMP; }

    public TextMeshProUGUI GetTerraCurrentHealthTMP() { return terraCurrentHealthTMP; }

    public TextMeshProUGUI GetTerraMaxHealthTMP() { return terraMaxHealthTMP; }

    public float GetCurrentHealth() { return currentHealth; }
}
