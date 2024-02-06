using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Confusion")]
public class ConfusionVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new ConfusionVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class ConfusionVolatileStatusEffect : VolatileStatusEffectBase
{
    private static readonly int MIN_TURN_DURATION = 2;
    private static readonly int MAX_TURN_DURATION = 5;
    private static readonly int RECOIL_POWER = 40;

    private int turnDuration;
    private int turnCounter;

    public ConfusionVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 0;
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += TerraAttackConfusionActive;
        battleSystem.OnEndOfTurn += EndOfTurnCounterIncrement;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= TerraAttackConfusionActive;
        battleSystem.OnEndOfTurn -= EndOfTurnCounterIncrement;
    }

    private void TerraAttackConfusionActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.TerraIsConfusedMsg(terraBattlePosition.GetTerra()));
        if (turnCounter <= turnDuration) {
            int randConfusionRoll = Random.Range(0, 2);
            if (randConfusionRoll == 1) {
                int recoilDamage = (int)CombatCalculator.InitialAttackDamage(terraBattlePosition, terraBattlePosition, RECOIL_POWER);
                eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, recoilDamage);
                eventArgs.GetTerraAttack().SetCanceled(true);
            }
        }
        else {
            Debug.Log(BattleDialog.TerraSnappedOutOfConfusionMsg(terraBattlePosition.GetTerra()));
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
        }
    }

    private void EndOfTurnCounterIncrement(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
    }
}