using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Counter")]
public class CounterSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Counter(terraAttack, this);
    }
}

public class Counter : TerraMoveBase
{
    private static readonly float DAMAGE_MULTIPLIER = 2f;

    private int physicalDamageRecieved;

    public Counter(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO)
    {
        physicalDamageRecieved = 0;
    }

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamageByTerra += AccumulateDamage;
        battleSystem.OnTerraDamageByTerra += UnleashDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
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
        if (eventArgs.GetTerraAttack().GetMove().GetMoveSO().GetDamageType() != DamageType.PHYSICAL)
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