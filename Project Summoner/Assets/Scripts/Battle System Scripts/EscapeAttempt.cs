using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttempt
{
    private bool isPrimarySide;
    private float escapeModifier;
    private bool isSuccessful;

    public EscapeAttempt(bool isPrimarySide)
    {
        this.isPrimarySide = isPrimarySide;
        escapeModifier = 1f;
        isSuccessful = false;
    }

    public bool IsPrimarySide() { return isPrimarySide; }

    public float GetEscapeModifier() { return escapeModifier; }

    public void SetEscapeModifier(float escapeModifier) {  this.escapeModifier = escapeModifier; }

    public bool IsSuccessful() { return isSuccessful; }

    public void SetSuccessful(bool isSuccessful) {  this.isSuccessful = isSuccessful; }
}
