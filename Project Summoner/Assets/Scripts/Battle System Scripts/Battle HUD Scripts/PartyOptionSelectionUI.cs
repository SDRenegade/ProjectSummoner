using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyOptionSelectionUI : MonoBehaviour
{
    [SerializeField] private Button backgroundPanel;
    [SerializeField] private RectTransform buttonPanel;
    [SerializeField] private Button summaryBtn;
    [SerializeField] private Button switchBtn;
    [SerializeField] private Button cancelBtn;

    public void Start()
    {
        backgroundPanel.onClick.AddListener(delegate {
            CloseOptionSelection();
        });

        cancelBtn.onClick.AddListener(delegate {
            CloseOptionSelection();
        });
    }

    public void OpenOptionSelctionUI(TerraBattlePosition activeTerraPosition, int selectedTerraIndex, Action<TerraBattlePosition, TerraSwitch> switchAction, BattleSystem battleSystem)
    {
        RemoveListeners();
        Vector3 optionSelectionPosition = new Vector3(
            Input.mousePosition.x + (buttonPanel.sizeDelta.x / 2),
            Input.mousePosition.y - (buttonPanel.sizeDelta.y / 2),
            0);
        buttonPanel.gameObject.transform.position = optionSelectionPosition;

        summaryBtn.onClick.AddListener(delegate {
            Debug.Log("Summary action selected for " + battleSystem.GetPrimaryTerraList()[selectedTerraIndex]);
            CloseOptionSelection();
        });
        
        if(IsValidSwitchIndex(selectedTerraIndex, battleSystem)) {
            switchBtn.gameObject.SetActive(true);
            switchBtn.onClick.AddListener(delegate {
                Debug.Log("Switching action selected for " + activeTerraPosition.GetTerra() + " and " + battleSystem.GetPrimaryTerraList()[selectedTerraIndex]);
                CloseOptionSelection();
                battleSystem.ExitPartyMenuUI();
                TerraBattlePosition[] terraBattlePositionArr = battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
                TerraSwitch terraSwitch = new TerraSwitch(Array.IndexOf(terraBattlePositionArr, activeTerraPosition), selectedTerraIndex, true);
                switchAction?.Invoke(activeTerraPosition, terraSwitch);
            });

            /*
            if(isMustSwitch) {
                switchBtn.onClick.AddListener(delegate {
                    Debug.Log("Force switch performed for " + battleSystem.GetBattleActionManager().GetCurrentTerraActionSelection().GetTerra() + " and " + battleSystem.GetPrimaryTerraList()[terraPartyIndex]);
                    CloseOptionSelection();
                    battleSystem.ExitPartyMenuUI();
                    TerraBattlePosition[] terraBattlePositionArr = battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
                    TerraSwitch terraSwitch = new TerraSwitch(Array.IndexOf(terraBattlePositionArr, activeTerraPosition), terraPartyIndex, true);
                    battleSystem.SwitchTerra(terraSwitch);
                });
            }
            else {
                switchBtn.onClick.AddListener(delegate {
                    Debug.Log("Switching action selected for " + battleSystem.GetBattleActionManager().GetCurrentTerraActionSelection().GetTerra() + " and " + battleSystem.GetPrimaryTerraList()[terraPartyIndex]);
                    CloseOptionSelection();
                    battleSystem.ExitPartyMenuUI();
                    TerraBattlePosition[] terraBattlePositionArr = battleSystem.GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
                    TerraSwitch terraSwitch = new TerraSwitch(Array.IndexOf(terraBattlePositionArr, activeTerraPosition), terraPartyIndex, true);
                    battleSystem.SwitchTerraSelection(new SwitchBattleAction(activeTerraPosition, terraSwitch));
                });
            }*/
        }
        else
            switchBtn.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    private bool IsValidSwitchIndex(int terraPartyIndex, BattleSystem battleSystem)
    {
        if (terraPartyIndex < battleSystem.GetBattleFormat().NumberOfLeadingPositions())
            return false;
        foreach(TerraSwitch terraSwitch in battleSystem.GetBattleActionManager().GetTerraSwitchList()) {
            if (!terraSwitch.IsPrimarySide())
                continue;

            if (terraPartyIndex == terraSwitch.GetBenchPositionIndex())
                return false;
        }

        return true;
    }

    private void RemoveListeners()
    {
        summaryBtn.onClick.RemoveAllListeners();
        switchBtn.onClick.RemoveAllListeners();
    }

    public void CloseOptionSelection()
    {
        RemoveListeners();
        gameObject.SetActive(false);
    }
}
