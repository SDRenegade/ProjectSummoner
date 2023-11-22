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

    //TODO Add checks for attackers accuracy and defenders evasion
    public static bool HitCheck(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, TerraMoveBase moveBase)
    {
        int hitIndex = Random.Range(0, 100);
        return (moveBase.GetBaseAccuracy().GetIsNull() || hitIndex <= moveBase.GetBaseAccuracy().GetValue());
    }

    public static int CalculateDamage(TerraBattlePosition attackingTerraPosition, TerraBattlePosition targetTerraPosition, TerraMoveBase moveBase)
    {
        DamageType damageType = moveBase.GetDamageType();
        if (damageType == DamageType.STATUS)
            return 0;

        int attackingStat = (damageType == DamageType.PHYSICAL_DAMAGE) ? attackingTerraPosition.GetTerra().GetAttack() : attackingTerraPosition.GetTerra().GetSpAttack();
        int defendingStat = (damageType == DamageType.PHYSICAL_DAMAGE) ? attackingTerraPosition.GetTerra().GetDefence() : attackingTerraPosition.GetTerra().GetSpDefence();
        StatStages attackStage = (damageType == DamageType.PHYSICAL_DAMAGE) ? attackingTerraPosition.GetAttackStage() : attackingTerraPosition.GetSpAttackStage();
        StatStages defenceStage = (damageType == DamageType.PHYSICAL_DAMAGE) ? targetTerraPosition.GetDefenceStage() : targetTerraPosition.GetSpDefenceStage();

        int totalDamage = (int)(((2 * attackingTerraPosition.GetTerra().GetLevel() / 5f) + 2) * moveBase.GetBaseDamage().GetValue() * ((attackingStat * attackStage.GetStatStageMultiplier()) / (defendingStat * defenceStage.GetStatStageMultiplier())) / 50f + 2);
        totalDamage = (int)(totalDamage * moveBase.GetMoveType().GetTypeEffectiveness(targetTerraPosition.GetTerra().GetTerraBase().GetTerraTypes()));

        return totalDamage > 0 ? totalDamage : 1;
    }

}
