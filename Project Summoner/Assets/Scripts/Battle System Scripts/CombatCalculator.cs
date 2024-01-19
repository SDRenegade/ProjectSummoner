using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    PHYSICAL,
    SPECIAL,
    STATUS
}

public static class CombatCalculator
{
    public static readonly float STAB_BONUS = 1.50f;
    public static readonly float HIGHEST_DAMAGE_ROLL = 1f;
    public static readonly float LOWEST_DAMAGE_ROLL = 0.85f;
    public static readonly float CRIT_CHANCE = 1/16f;
    public static readonly float CRIT_MULTIPLIER = 2f;

    public static bool HitCheck(DirectAttackParams directAttackParams)
    {
        if (directAttackParams.IsMustHit())
            return true;
        if (directAttackParams.GetDefenderPosition().GetBattlePositionState() != BattlePositionState.NORMAL)
            return false;

        int randomHitIndex = Random.Range(0, 100);
        int attackAccuracyIndex = (int)(directAttackParams.GetMove().GetMoveBase().GetBaseAccuracy().GetValue() / StatStagesExtension.GetStatStageMultiplier(directAttackParams.GetDefenderPosition().GetStatStage(Stats.EVA)) * directAttackParams.GetAccuracyModifier());

        return directAttackParams.GetMove().GetMoveBase().GetBaseAccuracy().IsNull() || attackAccuracyIndex > randomHitIndex;
    }

    public static bool CriticalHitCheck(DirectAttackParams directAttackParams)
    {
        float randomCritIndex = Random.Range(0f, 1f);

        return randomCritIndex <= CRIT_CHANCE * directAttackParams.GetCritModifier();
    }

    //TODO Add logic for none effective moves that should deal no damage
    public static int? CalculateDamage(DirectAttackParams directAttackParams, bool isCrit)
    {
        DamageType damageType = directAttackParams.GetMove().GetMoveBase().GetDamageType();
        TerraMoveBase terraMoveBase = directAttackParams.GetMove().GetMoveBase();
        if (terraMoveBase.GetBaseDamage().IsNull() || damageType == DamageType.STATUS)
            return null;

        TerraBattlePosition attackerPosition = directAttackParams.GetAttackerPosition();
        TerraBattlePosition defenderPosition = directAttackParams.GetDefenderPosition();

        //Initial damage calculation without any modifiers
        int damage = (int)InitiaAttackDamage(attackerPosition, defenderPosition, terraMoveBase);
        //Move effectivness modifer
        damage = (int)(damage * terraMoveBase.GetMoveType().GetTypeEffectiveness(defenderPosition.GetTerra().GetTerraBase().GetTerraTypes()));
        //STAB condition modifier
        foreach(TerraType type in attackerPosition.GetTerra().GetTerraBase().GetTerraTypes()) {
            if(type == terraMoveBase.GetMoveType()) {
                damage = (int)(damage * STAB_BONUS);
                break;
            }
        }
        //Crit condition modifier
        if (isCrit)
            damage = (int)(damage * CRIT_MULTIPLIER);
        //Other damage modifier
        damage = (int)(damage * directAttackParams.GetDamageModifier());
        //Random damage percentage roll
        float damageRoll = Random.Range(LOWEST_DAMAGE_ROLL, HIGHEST_DAMAGE_ROLL);
        damage = (int)(damage * damageRoll);

        return damage > 0 ? damage : 1;
    }

    public static int? InitiaAttackDamage(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMoveBase terraMoveBase)
    {
        DamageType damageType = terraMoveBase.GetDamageType();
        if (terraMoveBase.GetBaseDamage().IsNull() || damageType == DamageType.STATUS)
            return null;

        int attackingStat = (damageType == DamageType.PHYSICAL) ? attackerPosition.GetTerra().GetAttack() : attackerPosition.GetTerra().GetSpAttack();
        int defendingStat = (damageType == DamageType.PHYSICAL) ? defenderPosition.GetTerra().GetDefence() : defenderPosition.GetTerra().GetSpDefence();
        StatStages attackStage = (damageType == DamageType.PHYSICAL) ? attackerPosition.GetStatStage(Stats.ATK) : attackerPosition.GetStatStage(Stats.ATK);
        StatStages defenceStage = (damageType == DamageType.PHYSICAL) ? defenderPosition.GetStatStage(Stats.DEF) : defenderPosition.GetStatStage(Stats.DEF);

        int damage = (int)(((2 * attackerPosition.GetTerra().GetLevel() / 5f) + 2) * terraMoveBase.GetBaseDamage().GetValue() * (attackingStat * attackStage.GetStatStageMultiplier() / (defendingStat * defenceStage.GetStatStageMultiplier())) / 50f + 2);

        return damage;
    }

    //This alternative version of the InitialAttackDamage method is used when calculating the damage of a
    //of a non existing move that always deals physical damage. Confusion damage uses this method.
    public static int? InitiaAttackDamage(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, int baseDamage)
    {
        if (baseDamage <= 0)
            return null;

        int attackingStat = attackerPosition.GetTerra().GetAttack();
        int defendingStat = defenderPosition.GetTerra().GetDefence();
        StatStages attackStage = attackerPosition.GetStatStage(Stats.ATK);
        StatStages defenceStage = defenderPosition.GetStatStage(Stats.DEF);

        int damage = (int)(((2 * attackerPosition.GetTerra().GetLevel() / 5f) + 2) * baseDamage * (attackingStat * attackStage.GetStatStageMultiplier() / (defendingStat * defenceStage.GetStatStageMultiplier())) / 50f + 2);

        return damage;
    }
}
