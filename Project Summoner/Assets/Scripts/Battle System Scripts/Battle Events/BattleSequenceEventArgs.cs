using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceEventArgs : BattleEventArgs
{
    private IBattleSequence battleSequence;

    public BattleSequenceEventArgs(IBattleSequence battleSequence, BattleSystem battleSystem) : base(battleSystem)
    {
        this.battleSequence = battleSequence;
    }

    public IBattleSequence GetBattleSequence() { return battleSequence; }
}
