using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDialog
{
    public static readonly string ESCAPE_ATTEMPT_SUCCESS = "You have escaped from battle.";
    public static readonly string ESCAPE_ATTEMPT_FAILED = "You were unable to flee.";
    public static readonly string CANNOT_ESCAPE_SUMMONER_BATTLE = "You cannot escape from a summoner battle.";
    public static readonly string MULTIPLE_ESCAPE_ATTEMPTS = "You can only attempt to escape from battle one a turn.";
    public static readonly string NOT_VERY_EFFECTIVE_ATTACK = "The attack was not very effective.";
    public static readonly string SUPER_EFFECTIVE_ATTACK = "The attack was super effective!";
    public static readonly string ATTACK_MISSED = "The attack missed.";
    public static readonly string ATTACK_FAILED = "But it failed.";
    public static readonly string CRITICAL_HIT = "It was a critical hit!";

    // ================== Combat Messages ==================
    public static string SwitchTerraMsg(TerraSwitch terraSwitch, List<Terra> terraList) {
        return terraList[terraSwitch.GetLeadingPositionIndex()] + " has been switched out for " + terraList[terraSwitch.GetBenchPositionIndex()];
    }

    public static string AttackUsedMsg(Terra attackingTerra, TerraMove move) {
        return attackingTerra + " has used " + move;
    }

    public static string AttackUsedMsg(TerraAttack terraAttack) {
        return terraAttack.GetAttackerPosition().GetTerra() + " has used " + terraAttack.GetMove();
    }

    public static string TerraDamagedMsg(Terra terra, int damage) {
            return terra + " has taken " + damage + " damage";
    }

    public static string TerraHealedMsg(Terra terra, int healAmt) {
        return terra + " has recovered " + healAmt + " health";
    }

    public static string StatStageChangeMsg(Terra terra, Stats stat, StatStages currentStatStage, int modification) {
        if (modification == 0)
            return terra + "'s " + stat + " has not changed.";
        else if(modification > 0) {
            if(modification == 1)
                return terra + "'s " + stat + " has rose to " + currentStatStage;
            else
                return terra + "'s " + stat + " has sharply rose to " + currentStatStage;
        }
        else {
            if (modification == -1)
                return terra + "'s " + stat + " has lowerd to " + currentStatStage;
            else
                return terra + "'s " + stat + " has sharply lowered to " + currentStatStage;
        }
    }

    public static string ResetStatStagesMsg(Terra terra) {
        return "All stat stages have be reset to neutral for " + terra;
    }

    public static string TerraFaintedMsg(Terra terra) {
        return terra + " has fainted.";
    }

    public static string StatusInflictionMsg(Terra terra, StatusEffectSO statusEffectSO) {
        return terra + " has been inflicted with " + statusEffectSO;
    }

    public static string VolatileStatusInflictionMsg(Terra terra, VolatileStatusEffectSO vStatusEffectSO) {
        return terra + " has been inflicted with " + vStatusEffectSO;
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

    public static string ConfusionInflictedMsg(Terra terra) {
        return terra + " has been been confused.";
    }

    public static string TerraIsConfusedMsg(Terra terra) {
        return terra + " is confused.";
    }

    public static string ConfusionProkedMsg(Terra terra, int damage) {
        return terra + " hit themself for " + damage + " damage.";
    }

    public static string TerraSnappedOutOfConfusionMsg(Terra terra) {
        return terra + " has snapped out of confusion.";
    }

    public static string FlinchedMsg(Terra terra) {
        return terra + " has flinched.";
    }

    public static string AttackCharging(TerraMoveSO terraMoveSO) {
        return terraMoveSO + " is charging.";
    }

    public static string AttackRecharging(Terra terra) {
        return terra + " is recharging.";
    }

    public static string MultiHitMsg(Terra terra, int numHits) {
        return terra + " has landed " + numHits + " hits.";
    }

    public static string RecoilDamageMsg(Terra terra, int recoilDamage) {
        return terra + " has taken " + recoilDamage + " recoil damage.";
    }

    public static string ProtectActiveMsg(Terra terra) {
        return terra + " was unaffected due to Protect.";
    }

    public static string ProtectCooldownMsg(Terra terra) {
        return terra + " was unable to use protect.";
    }

    public static string BindDamageMsg(Terra terra, int bindDamage) {
        return terra + " has taken " + bindDamage + " damage from bind.";
    }

    public static string HighJumpKickMissedMsg(Terra terra, int recoilDamage) {
        return "High Jump Kick missed and " + terra + " has taken " + recoilDamage + " recoil damage.";
    }

    public static string LightScreenExpiredMsg(Terra terra) {
        return "Light screen has went down for " + terra + ".";
    }

    public static string DisableActiveMsg(Terra terra) {
        return terra + " has had a move disabled.";
    }

    public static string MetronomeMoveMsg(TerraMove move) {
        return "Metronome has turned into the move " + move;
    }

    public static string SubstituteExpired(Terra terra) {
        return "The substitute on " + terra + " has disappeared.";
    }

    public static string MistProkedMsg(Terra terra) {
        return terra + " is immune to stat changes with mist active.";
    }

    // ================== Item Messages ==================
    public static string ItemProkedMsg(ItemBase item) {
        return item + " has proked.";
    }

    public static string ItemConsumedMsg(ItemBase item) {
        return item + " has been consumed.";
    }

    // ================== UI Selection Messages ==================
    public static string NoMovePowerPointsLeftMsg(TerraMove move) {
        return move + " has no PP left.";
    }
}
