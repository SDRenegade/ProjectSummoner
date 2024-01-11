using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Fury Swipes")]
public class FurySwipesBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new FurySwipesAction(terraAttack);
    }
}

public class FurySwipesAction : TerraMoveAction
{
    private static readonly int MIN_HITS = 2;
    private static readonly int MAX_HITS = 5;

    private TerraAttack terraAttack;

    public FurySwipesAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += SetNumberOfHits;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
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