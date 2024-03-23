using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttempt
{
    private bool isPrimarySide;

    public EscapeAttempt(bool isPrimarySide)
    {
        this.isPrimarySide = isPrimarySide;
    }

    public bool IsPrimarySide() { return isPrimarySide; }
}
