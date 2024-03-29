using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Counter")]
public class CounterBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new CounterAction(terraAttack);
    }
}

public class CounterAction : TerraMoveAction
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    private TerraAttack terraAttack;
    private int physicalDamageRecieved;

    public CounterAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        physicalDamageRecieved = 0;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += AccumulateDamage;
        battleSystem.OnTerraDamageByTerra += UnleashDamage;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra -= AccumulateDamage;
        battleSystem.OnTerraDamageByTerra -= UnleashDamage;
    }

    public void AccumulateDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraAttack.GetAttackerPosition())
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;
        if (eventArgs.GetTerraAttack().GetMove().GetMoveBase().GetDamageType() != DamageType.PHYSICAL)
            return;

        physicalDamageRecieved += (int)eventArgs.GetDirectAttackLog().GetDamage();
    }

    public void UnleashDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (terraAttack != eventArgs.GetTerraAttack())
            return;

        if(physicalDamageRecieved <= 0)
            eventArgs.GetDirectAttackLog().SetDamage(null);
        else
            eventArgs.GetDirectAttackLog().SetDamage((int)(physicalDamageRecieved * DAMAGE_MULTIPLIER));

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}