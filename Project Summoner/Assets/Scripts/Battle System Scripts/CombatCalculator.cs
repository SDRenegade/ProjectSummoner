using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    PHYSICAL_DAMAGE,
    SPECIAL_DAMAGE,
    STATUS
}

public static class CombatCalculator
{
    public static readonly float STAB_BONUS = 1.50f;
    public static readonly float HIGHEST_DAMAGE_ROLL = 1f;
    public static readonly float LOWEST_DAMAGE_ROLL = 0.85f;
    public static readonly float CRIT_CHANCE = 1/16f;
    public static readonly float CRIT_MULTIPLIER = 2f;

    public static bool HitCheck(TerraAttackParams terraAttackParams)
    {
        int randomHitIndex = Random.Range(0, 100);

        return (terraAttackParams.GetMove().GetMoveBase().GetBaseAccuracy().IsNull() || randomHitIndex < terraAttackParams.CalculateAttackAccuracy());
    }

    public static bool CriticalHitCheck(TerraAttackParams terraAttackParams)
    {
        float randomCritIndex = Random.Range(0f, 1f);

        return randomCritIndex <= CRIT_CHANCE * terraAttackParams.GetCritModifier();
    }

    public static int? CalculateDamage(TerraAttackParams terraAttackParams, bool isCrit)
    {
        DamageType damageType = terraAttackParams.GetMove().GetMoveBase().GetDamageType();
        if (terraAttackParams.GetMove().GetMoveBase().GetBaseDamage().IsNull() || damageType == DamageType.STATUS)
            return null;

        TerraBattlePosition attackerPosition = terraAttackParams.GetAttackerPosition();
        TerraBattlePosition defenderPosition = terraAttackParams.GetDefenderPosition();

        int attackingStat = (damageType == DamageType.PHYSICAL_DAMAGE) ? attackerPosition.GetTerra().GetAttack() : attackerPosition.GetTerra().GetSpAttack();
        int defendingStat = (damageType == DamageType.PHYSICAL_DAMAGE) ? defenderPosition.GetTerra().GetDefence() : defenderPosition.GetTerra().GetSpDefence();
        StatStages attackStage = (damageType == DamageType.PHYSICAL_DAMAGE) ? attackerPosition.GetAttackStage() : attackerPosition.GetSpAttackStage();
        StatStages defenceStage = (damageType == DamageType.PHYSICAL_DAMAGE) ? defenderPosition.GetDefenceStage() : defenderPosition.GetSpDefenceStage();

        //Initial damage calculation without any modifiers
        int damage = (int)(((2 * attackerPosition.GetTerra().GetLevel() / 5f) + 2) * terraAttackParams.GetMove().GetMoveBase().GetBaseDamage().GetValue() * (attackingStat * attackStage.GetStatStageMultiplier() / (defendingStat * defenceStage.GetStatStageMultiplier())) / 50f + 2);
        //Move effectivness modifer
        damage = (int)(damage * terraAttackParams.GetMove().GetMoveBase().GetMoveType().GetTypeEffectiveness(defenderPosition.GetTerra().GetTerraBase().GetTerraTypes()));
        //STAB condition modifier
        foreach(TerraType type in attackerPosition.GetTerra().GetTerraBase().GetTerraTypes()) {
            if(type == terraAttackParams.GetMove().GetMoveBase().GetMoveType()) {
                damage = (int)(damage * STAB_BONUS);
                break;
            }
        }
        //Crit condition modifier
        if (isCrit)
            damage = (int)(damage * CRIT_MULTIPLIER);
        //Other damage modifier
        damage = (int)(damage * terraAttackParams.GetDamageModifier());
        //Random damage percentage roll
        float damageRoll = Random.Range(LOWEST_DAMAGE_ROLL, HIGHEST_DAMAGE_ROLL);
        damage = (int)(damage * damageRoll);

        return damage > 0 ? damage : 1;
    }
}
