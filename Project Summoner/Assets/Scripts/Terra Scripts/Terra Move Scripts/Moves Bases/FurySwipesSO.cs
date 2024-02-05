using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Fury Swipes")]
public class FurySwipesSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new FurySwipes(terraAttack, this);
    }
}

public class FurySwipes : TerraMoveBase
{
    private static readonly int MIN_HITS = 2;
    private static readonly int MAX_HITS = 5;

    public FurySwipes(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += SetNumberOfHits;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= SetNumberOfHits;
    }

    private void SetNumberOfHits(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;

        int numHits = Random.Range(MIN_HITS, MAX_HITS + 1);
        eventArgs.GetDirectAttackParams().SetHitCount(numHits);
        eventArgs.GetBattleSystem().OnDirectAttack -= SetNumberOfHits;
    }
}