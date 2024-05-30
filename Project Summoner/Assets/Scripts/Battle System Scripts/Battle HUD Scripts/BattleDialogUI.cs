using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform dialogBox;


    private void Start()
    {
        dialogBox = GetComponent<RectTransform>();
        HideDialog();
    }

    public void SetDialog(string dialog)
    {
        text.SetText(dialog);
        dialogBox.sizeDelta = new Vector2(dialogBox.sizeDelta.x, text.fontSize * 2 * text.textInfo.lineCount);

        ShowDialog();
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    public void HideDialog()
    {
        gameObject.SetActive(false);
    }
}
