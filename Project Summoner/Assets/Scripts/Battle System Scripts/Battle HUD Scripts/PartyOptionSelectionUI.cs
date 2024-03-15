using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Background clicked and canceled");
            CloseOptionSelection();
        });

        cancelBtn.onClick.AddListener(delegate {
            Debug.Log("Cancel button clicked");
            CloseOptionSelection();
        });
    }

    public void OpenOptionSelctionUI(Terra terra, int? terraPartyIndex, BattleSystem battleSystem)
    {
        RemoveListeners();
        Vector3 optionSelectionPosition = new Vector3(
            Input.mousePosition.x + (buttonPanel.sizeDelta.x / 2),
            Input.mousePosition.y - (buttonPanel.sizeDelta.y / 2),
            0);
        buttonPanel.gameObject.transform.position = optionSelectionPosition;

        summaryBtn.onClick.AddListener(delegate {
            Debug.Log("Summary for " + terra);
            CloseOptionSelection();
        });
        switchBtn.onClick.AddListener(delegate {
            Debug.Log("Switching with " + terra);
            CloseOptionSelection();
        });

        gameObject.SetActive(true);
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
