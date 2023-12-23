using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Fury Swipes")]
public class FurySwipesBase : TerraMoveBase
{
    private readonly int MIN_HITS = 2;
    private readonly int MAX_HITS = 5;

    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem) {}

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem)
    {
        int numHits = Random.Range(MIN_HITS, MAX_HITS + 1);
        terraAttackParams.SetHitCount(numHits);
    }
    
    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem) {}
}