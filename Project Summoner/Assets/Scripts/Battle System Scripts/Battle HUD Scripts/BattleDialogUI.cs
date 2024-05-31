using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetDialog(string dialog)
    {
        text.SetText(dialog);
        gameObject.SetActive(true);
    }
}
