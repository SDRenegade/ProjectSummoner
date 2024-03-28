using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAttemptEventArgs : BattleEventArgs
{
    private CaptureAttempt captureAttempt;
    private bool isCanceled;

    public CaptureAttemptEventArgs(CaptureAttempt captureAttempt, BattleSystem battleSystem) : base(battleSystem)
    {
        this.captureAttempt = captureAttempt;
    }

    public CaptureAttempt GetCaptureAttempt() { return this.captureAttempt; }

    public bool IsCanceled() { return this.isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
