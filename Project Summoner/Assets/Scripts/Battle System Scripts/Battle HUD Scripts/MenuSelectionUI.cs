using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelectionUI : MonoBehaviour
{
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button partyBtn;
    [SerializeField] private Button escapeBtn;
    [SerializeField] private Button exitActionBtn;

    public void OpenMenuSelectionUI(BattleActionManager battleActionManager)
    {
        gameObject.SetActive(true);
        if (battleActionManager.GetSelectedActionStack().Count > 0)
            exitActionBtn.interactable = true;
        else
            exitActionBtn.interactable = false;
    }
}
