using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDialog
{
    public static readonly string ATTEMPT_ESCAPE_SUCCESS = "You have escaped from battle.";
    public static readonly string ATTEMPT_ESCAPE_FAIL = "You were unable to flee.";
    public static readonly string ATTEMPT_ESCAPE_SUMMONER_BATTLE = "You cannot flee from a summoner battle.";
    public static readonly string NOT_VERY_EFFECTIVE_ATTACK = "The attack was not very effective.";
    public static readonly string SUPER_EFFECTIVE_ATTACK = "The attack was super effective!";
    public static readonly string ATTACK_MISSED = "The attack missed.";
    public static readonly string ATTACK_FAILED = "But it failed.";
    public static readonly string CRITICAL_HIT = "It was a critical hit!";

    // ================== Combat Messages ==================
    public static string AttackUsedMsg(Terra attackingTerra, TerraMove move) {
        return attackingTerra + " has used " + move;
    }

    public static string AttackUsedMsg(TerraAttack terraAttack) {
        return terraAttack.GetAttackerPosition().GetTerra() + " has used " + terraAttack.GetMove();
    }

    public static string DamageDealtMsg(Terra attackingTerra, Terra defendingTerra, int damage) {
        return attackingTerra + " has dealt " + damage + " to " + defendingTerra;
    }

    public static string HealthHealedMsg(Terra terra, int healAmt) {
        return terra + " has healed for " + healAmt;
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

    public static string ResetStatStagesMsg(Terra terra) {
        return "All stat stages have be reset to neutral for " + terra;
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

    public static string FlinchedMsg(Terra terra) {
        return terra + " has flinched.";
    }

    public static string MultiHitMsg(Terra terra, int numHits) {
        return terra + " has landed " + numHits + " hits";
    }

    public static string RecoilDamageMsg(Terra terra, int recoilDamage) {
        return terra + " has taken " + recoilDamage + " recoil damage.";
    }

    // ================== UI Selection Messages ==================
    public static string NoMovePowerPointsLeftMsg(TerraMove move) {
        return move + " has no PP left.";
    }
}
