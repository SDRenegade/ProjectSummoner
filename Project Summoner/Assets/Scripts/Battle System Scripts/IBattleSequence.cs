using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleSequence
{
    Dictionary<Action, float> GetTasksByTime(BattleStage battleStage, BattleCamera battleCam, out float sequenceDuration);
}
