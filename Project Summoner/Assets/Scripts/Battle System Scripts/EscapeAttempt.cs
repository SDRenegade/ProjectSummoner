using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttempt
{
    private bool isPrimarySide;
    private float escapeModifier;

    public EscapeAttempt(bool isPrimarySide)
    {
        this.isPrimarySide = isPrimarySide;
        escapeModifier = 1f;
    }

    public bool IsPrimarySide() { return isPrimarySide; }

    public float GetEscapeModifier() { return escapeModifier; }

    public void SetEscapeModifier(float escapeModifier) {  this.escapeModifier = escapeModifier; }
}
