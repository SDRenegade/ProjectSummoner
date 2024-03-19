using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Crab Hammer")]
public class CrabHammerSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new CrabHammer(terraAttack, this);
    }
}

public class CrabHammer : TerraMoveBase
{
    private static readonly float CRIT_MODIFIER = 1.2f;

    public CrabHammer(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += AddCritModifier;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
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