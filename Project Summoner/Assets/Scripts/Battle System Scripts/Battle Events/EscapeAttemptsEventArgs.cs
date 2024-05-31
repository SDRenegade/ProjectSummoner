using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttemptsEventArgs : BattleEventArgs
{
    private EscapeAttempt escapeAttempt;
    private bool isGuaranteedEscape;
    private bool isCanceled;

    public EscapeAttemptsEventArgs(EscapeAttempt escapeAttempt, BattleSystem battleSystem) : base(battleSystem)
    {
        this.escapeAttempt = escapeAttempt;
        isGuaranteedEscape = false;
        isCanceled = false;
    }

    public EscapeAttempt GetEscapeAttempt() { return escapeAttempt; }

    public bool IsGuaranteedEscape() { return isGuaranteedEscape; }

    public void SetGuaranteedEscape(bool mustHit) { isGuaranteedEscape = mustHit; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
