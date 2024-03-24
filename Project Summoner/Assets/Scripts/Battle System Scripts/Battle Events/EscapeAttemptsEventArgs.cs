using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttemptsEventArgs : BattleEventArgs
{
    private EscapeAttempt escapeAttempt;
    private bool isMustHit;
    private bool isCanceled;

    public EscapeAttemptsEventArgs(EscapeAttempt escapeAttempt, BattleSystem battleSystem) : base(battleSystem)
    {
        this.escapeAttempt = escapeAttempt;
        isMustHit = false;
        isCanceled = false;
    }

    public EscapeAttempt GetEscapeAttempt() { return escapeAttempt; }

    public bool IsMustHit() { return isMustHit; }

    public void SetMustHit(bool mustHit) {  isMustHit = mustHit; }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
