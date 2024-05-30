using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAttemptBattleAction : BattleAction
{
    private CaptureAttempt captureAttempt;

    public CaptureAttemptBattleAction(TerraBattlePosition terraBattlePosition, CaptureAttempt captureAttempt) : base(terraBattlePosition)
    {
        this.captureAttempt = captureAttempt;
    }

    public override void Execute(BattleActionManager battleActionManager)
    {
        battleActionManager.SetCaptureAttempt(captureAttempt);
    }

    public override void Undo(BattleActionManager battleActionManager)
    {
        battleActionManager.SetCaptureAttempt(null);
    }
}
