using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Crab Hammer")]
public class CrabHammerBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new CrabHammerAction(terraAttack);
    }
}

public class CrabHammerAction : TerraMoveAction
{
    private static readonly float CRIT_MODIFIER = 1.2f;

    private TerraAttack terraAttack;

    public CrabHammerAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += AddCritModifier;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddCritModifier;
    }

    private void AddCritModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (terraAttack.GetMove() != eventArgs.GetDirectAttackParams().GetMove())
            return;

        eventArgs.GetDirectAttackParams().AddCritModifier(CRIT_MODIFIER);
    }
}