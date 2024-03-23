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
    public static readonly float STAB_BONUS = 1.5f;
    public static readonly float HIGHEST_DAMAGE_ROLL = 1f;
    public static readonly float LOWEST_DAMAGE_ROLL = 0.85f;
    public static readonly float CRIT_CHANCE = 1/16f;
    public static readonly float CRIT_MULTIPLIER = 2f;

    public static bool HitCheck(DirectAttackParams directAttackParams)
    {
        if (directAttackParams.GetDefenderPosition().GetBattlePositionState() != BattlePositionState.NORMAL)
            return false;
        if (directAttackParams.IsMustHit() || directAttackParams.GetMove().GetMoveSO().GetBaseAccuracy().IsNull())
            return true;

        float randomHitIndex = Random.Range(0, 1f);
        float moveAccuracy = (directAttackParams.GetMove().GetMoveSO().GetBaseAccuracy().GetValue() * 0.01f) * StatStagesExtension.GetStatStageMultiplier(directAttackParams.GetAttackerPosition().GetStatStage(Stats.ACC)) * directAttackParams.GetAccuracyModifier();
        float defenderEvasiveness = StatStagesExtension.GetStatStageMultiplier(directAttackParams.GetDefenderPosition().GetStatStage(Stats.EVA)) * directAttackParams.GetEvasivenessModifier();
        float attackAccuracyIndex = moveAccuracy / defenderEvasiveness;

        return attackAccuracyIndex > randomHitIndex;
    }

    public static bool CriticalHitCheck(DirectAttackParams directAttackParams)
    {
        float randomCritIndex = Random.Range(0f, 1f);

        return randomCritIndex <= CRIT_CHANCE * directAttackParams.GetCritModifier();
    }

    public static int? DamageCalculation(DirectAttackParams directAttackParams, bool isCrit)
    {
        DamageType damageType = directAttackParams.GetMove().GetMoveSO().GetDamageType();
        TerraMoveSO terraMoveBase = directAttackParams.GetMove().GetMoveSO();
        if (terraMoveBase.GetBaseDamage().IsNull() || damageType == DamageType.STATUS)
            return null;
        float typeEffectivenessModifier = directAttackParams.GetMoveTerraType().GetTypeEffectivenessModifier(directAttackParams.GetDefenderTerraTypeList());
        if (typeEffectivenessModifier == 0)
            return null;
        
        //Initial damage calculation without any modifiers
        float damage = (float)InitialAttackDamage(directAttackParams.GetAttackerPosition(), directAttackParams.GetDefenderPosition(), terraMoveBase);
        //Move effectivness modifer
        damage *= typeEffectivenessModifier;
        //STAB condition modifier
        foreach(TerraType type in directAttackParams.GetAttackerTerraTypeList()) {
            if(type == directAttackParams.GetMoveTerraType()) {
                damage *= STAB_BONUS;
                break;
            }
        }
        //Crit condition modifier
        if (isCrit)
            damage *= CRIT_MULTIPLIER;
        //Other damage modifier
        damage *= directAttackParams.GetDamageModifier();
        //Random damage percentage roll
        float damageRoll = Random.Range(LOWEST_DAMAGE_ROLL, HIGHEST_DAMAGE_ROLL);
        damage *= damageRoll;

        return (int)(damage > 0 ? damage : 1);
    }

    public static float? InitialAttackDamage(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, TerraMoveSO terraMoveBase)
    {
        DamageType damageType = terraMoveBase.GetDamageType();
        if (terraMoveBase.GetBaseDamage().IsNull() || damageType == DamageType.STATUS)
            return null;

        int attackingStat = (damageType == DamageType.PHYSICAL) ? attackerPosition.GetTerra().GetAttack() : attackerPosition.GetTerra().GetSpAttack();
        int defendingStat = (damageType == DamageType.PHYSICAL) ? defenderPosition.GetTerra().GetDefence() : defenderPosition.GetTerra().GetSpDefence();
        StatStages attackStage = (damageType == DamageType.PHYSICAL) ? attackerPosition.GetStatStage(Stats.ATK) : attackerPosition.GetStatStage(Stats.ATK);
        StatStages defenceStage = (damageType == DamageType.PHYSICAL) ? defenderPosition.GetStatStage(Stats.DEF) : defenderPosition.GetStatStage(Stats.DEF);

        float damage = ((2 * attackerPosition.GetTerra().GetLevel() / 5f) + 2) * terraMoveBase.GetBaseDamage().GetValue() * (attackingStat * attackStage.GetStatStageMultiplier() / (defendingStat * defenceStage.GetStatStageMultiplier())) / 50f + 2;

        return damage;
    }

    //This alternative version of the InitialAttackDamage method is used when calculating the damage of a
    //of a non existing move that always deals physical damage. Confusion damage uses this method.
    public static int? InitialAttackDamage(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, int baseDamage)
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

    public static bool EscapeAttemptCalculation(List<Terra> playerTerraList, List<Terra> wildTerraList)
    {
        int highestPlayerTerraSpeed = 0;
        for(int i = 0; i < playerTerraList.Count; i++) {
            if (playerTerraList[i].GetSpeed() > highestPlayerTerraSpeed)
                highestPlayerTerraSpeed = playerTerraList[i].GetSpeed();
        }

        int highestWildTerraSpeed = 0;
        for (int i = 0; i < wildTerraList.Count; i++) {
            if (wildTerraList[i].GetLevel() > highestWildTerraSpeed)
                highestWildTerraSpeed = wildTerraList[i].GetSpeed();
        }

        if (highestPlayerTerraSpeed >= highestWildTerraSpeed)
            return true;

        float escapeOdds = ((float)highestPlayerTerraSpeed * 32 / ((float)highestWildTerraSpeed / 4) + 30) / 256;

        return escapeOdds >= Random.Range(0f, 1f);
    }
}
