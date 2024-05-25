using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleSequence
{
    Dictionary<Action, float> GetBattleSequence(BattleStage battleStage, BattleCamera battleCam);
}
