using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDialog
{
    public static readonly string ATTEMPT_ESCAPE_SUCCESS = "You have escaped from battle.";
    public static readonly string ATTEMPT_ESCAPE_FAIL = "You were unable to flee.";
    public static readonly string NOT_VERY_EFFECTIVE_ATTACK = "The attack was not very effective.";
    public static readonly string SUPER_EFFECTIVE_ATTACK = "The attack was super effective!";
    public static readonly string ATTACK_MISSED = "The attack missed.";
    public static readonly string ATTACK_FAILED = "But it failed.";

    // ================== Combat Messages ==================
    public static string AttackUsedMsg(Terra attackingTerra, string moveName) {
        return attackingTerra + " has used " + moveName;
    }

    public static string DamageDealtMsg(Terra attackingTerra, Terra defendingTerra, int damage) {
        return attackingTerra + "has dealt " + damage + " to " + defendingTerra;
    }

    public static string StatStageIncrementMsg(Terra terra, StatStages currentStatStage, int statAdjustment)
    {
        if(statAdjustment == 0)
            return terra + "'s stat can no longer be increased.";

        string statAdjustmentDescription = (statAdjustment == 1) ? "rose" : "sharply rose";

        return terra + "'s stat has " + statAdjustmentDescription + " to " + currentStatStage;
    }

    public static string StatStageDecrementMsg(Terra terra, StatStages currentStatStage, int statAdjustment)
    {
        if (statAdjustment == 0)
            return terra + "'s stat can no longer be decreased.";

        string statAdjustmentDescription = (statAdjustment == 1) ? "lowered" : "sharply lowered";

        return terra + "'s stat has " + statAdjustmentDescription + " to " + currentStatStage;
    }

    public static string TerraFaintedMsg(Terra terra) {
        return terra + " has fainted.";
    }

    public static string ParalysisInflictedMsg(Terra terra) {
        return terra + " has been paralyzed.";
    }

    public static string BurnInflictedMsg(Terra terra) {
        return terra + " has been burned.";
    }

    public static string SleepInflictedMsg(Terra terra) {
        return terra + " has fallen asleep.";
    }

    public static string FreezeInflictedMsg(Terra terra) {
        return terra + " has been frozen.";
    }

    public static string BlightInflictedMsg(Terra terra) {
        return terra + " has been inflicted with blight.";
    }

    public static string ParalysisProkedMsg(Terra terra) {
        return terra + " is paralyzed and unable to move this turn.";
    }

    public static string BurnProkedMsg(Terra terra, int burnDamage) {
        return terra + " has been burned for " + burnDamage + " damage.";
    }

    public static string SleepProkedMsg(Terra terra) {
        return terra + " is fast asleep.";
    }

    public static string TerraWokeUpMsg(Terra terra) {
        return terra + " has woken up.";
    }

    public static string FreezeProkedMsg(Terra terra) {
        return terra + " is frozen and unable to move this turn.";

    }

    public static string TerraThawedMsg(Terra terra) {
        return terra + " has thawed.";
    }

    public static string BlightProkedMsg(Terra terra, int blightDamage) {
        return terra + " has been inflicted by blight for " + blightDamage + " damage.";
    }

    // ================== UI Selection Messages ==================

}
