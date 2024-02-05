using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Bide")]
public class BideSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Bide(terraAttack, this);
    }
}

public class Bide : TerraMoveBase
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    private int totalDamageRecieved;
    private int turnCounter;

    public Bide(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO)
    {
        totalDamageRecieved = 0;
        turnCounter = 0;
    }

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(true);
        battleSystem.OnTerraDamageByTerra += AccumulateDamage;
        battleSystem.OnEnteringActionSelection += QueueNextAttack;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        terraAttack.SetPersistent(false);
        battleSystem.OnTerraDamageByTerra -= AccumulateDamage;
        battleSystem.OnEnteringActionSelection -= QueueNextAttack;
        battleSystem.OnTerraDamageByTerra -= UnleashDamage;
    }

    private void QueueNextAttack(object sender, EnteringActionSelectionEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraAttack.GetAttackerPosition())
            return;

        turnCounter++;
        eventArgs.SetSkipActionSelection(true);

        if(turnCounter == 2) {
            eventArgs.GetBattleSystem().OnEnteringActionSelection -= QueueNextAttack;
            eventArgs.GetBattleSystem().OnTerraDamageByTerra += UnleashDamage;
        }
    }

    private void AccumulateDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraAttack.GetAttackerPosition())
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        totalDamageRecieved += (int)eventArgs.GetDirectAttackLog().GetDamage();
    }

    private void UnleashDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        eventArgs.GetDirectAttackLog().SetDamage((int)(totalDamageRecieved * DAMAGE_MULTIPLIER));

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}