using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SummonerDieBase : ItemBase
{
    public SummonerDieBase(ItemSO itemSO) : base(itemSO) {}

    public abstract float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem);
}
