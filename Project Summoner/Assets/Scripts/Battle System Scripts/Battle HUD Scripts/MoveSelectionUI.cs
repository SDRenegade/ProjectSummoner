using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelectionUI : MonoBehaviour
{
    public static readonly string EMPTY_SLOT_NAME = "--  --";

    [SerializeField] private Button[] moveBtns;
    [SerializeField] private Button exitBtn;

    public Button[] GetMoveBtns() { return moveBtns; }

    public Button GetExitBtn() { return exitBtn; }
}
