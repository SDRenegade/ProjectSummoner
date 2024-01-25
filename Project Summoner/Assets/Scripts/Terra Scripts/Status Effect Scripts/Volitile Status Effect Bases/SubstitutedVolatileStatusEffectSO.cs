using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Substituted")]
public class SubstitutedVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new SubstitutedVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class SubstitutedVolatileStatusEffect : VolatileStatusEffectBase
{
    private static readonly float PERCENT_MAX_HEALTH_SUBSTITUTE = 1/2f;

    private int substituteCurrentHealth;

    public SubstitutedVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        substituteCurrentHealth = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_SUBSTITUTE);
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamaged += AbsorbDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraDamaged -= AbsorbDamage;
    }

    public void AbsorbDamage(object sender, TerraDamagedEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDamage() == null)
            return;

        substituteCurrentHealth -= (int)eventArgs.GetDamage();
        eventArgs.SetDamage(null);
        if (substituteCurrentHealth <= 0) {
            Debug.Log(BattleDialog.SubstituteExpired(terraBattlePosition.GetTerra()));
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
        }
    }
}