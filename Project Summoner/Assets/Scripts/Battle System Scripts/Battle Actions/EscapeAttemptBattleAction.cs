using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAttemptBattleAction : BattleAction
{
    private EscapeAttempt escapeAttempt;

    public EscapeAttemptBattleAction(TerraBattlePosition terraBattlePosition, EscapeAttempt escapeAttempt) : base(terraBattlePosition)
    {
        this.escapeAttempt = escapeAttempt;
    }

    public override void Execute(BattleActionManager battleActionManager)
    {
        battleActionManager.SetEscapeAttempt(escapeAttempt);
    }

    public override void Undo(BattleActionManager battleActionManager)
    {
        battleActionManager.SetEscapeAttempt(null);
    }
}
