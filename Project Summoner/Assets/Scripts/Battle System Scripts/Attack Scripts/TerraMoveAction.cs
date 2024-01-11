using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TerraMoveAction : BattleAction
{
    void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem);
}
