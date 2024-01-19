using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Substituted")]
public class SubstitutedVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new SubstitutedVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class SubstitutedVolatileStatusEffectAction : BattleAction
{
    private static readonly float PERCENT_MAX_HEALTH_SUBSTITUTE = 1/2f;

    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private int substituteCurrentHealth;

    public SubstitutedVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        substituteCurrentHealth = (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_SUBSTITUTE);
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraHealthUpdate += AbsorbDamage;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnTerraHealthUpdate -= AbsorbDamage;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    public void AbsorbDamage(object sender, TerraHealthUpdateEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (eventArgs.GetHealthUpdate() == null)
            return;

        substituteCurrentHealth -= (int)eventArgs.GetHealthUpdate();
        eventArgs.SetHealthUpdate(null);
        if (substituteCurrentHealth <= 0) {
            RemoveBattleActions(eventArgs.GetBattleSystem());
            Debug.Log(BattleDialog.SubstituteExpired(terraBattlePosition.GetTerra()));
        }
    }
}