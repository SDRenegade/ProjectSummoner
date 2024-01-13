using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Confusion")]
public class ConfusionVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new ConfusionVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class ConfusionVolatileStatusEffectAction : BattleAction
{
    private static readonly int MIN_TURN_DURATION = 2;
    private static readonly int MAX_TURN_DURATION = 5;
    private static readonly int RECOIL_POWER = 40;

    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private int turnDuration;
    private int turnCounter;

    public ConfusionVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += TerraAttackConfusionActive;
        battleSystem.OnEndOfTurn += EndOfTurnCounterIncrement;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= TerraAttackConfusionActive;
        battleSystem.OnEndOfTurn -= EndOfTurnCounterIncrement;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    private void TerraAttackConfusionActive(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.TerraIsConfusedMsg(terraBattlePosition.GetTerra()));
        if (turnCounter <= turnDuration) {
            int randConfusionRoll = Random.Range(0, 2);
            if(randConfusionRoll == 1) {
                int recoilDamage = (int)CombatCalculator.InitiaAttackDamage(terraBattlePosition, terraBattlePosition, RECOIL_POWER);
                Debug.Log(BattleDialog.ConfusionProkedMsg(terraBattlePosition.GetTerra(), recoilDamage));
                terraBattlePosition.GetTerra().TakeDamage(recoilDamage);
                eventArgs.GetTerraAttack().SetCanceled(true);
            }
        }
        else {
            RemoveBattleActions(eventArgs.GetBattleSystem());
            Debug.Log(BattleDialog.TerraSnappedOutOfConfusionMsg(terraBattlePosition.GetTerra()));
        }
    }

    private void EndOfTurnCounterIncrement(object sender, BattleEventArgs eventArgs)
    {
        turnCounter++;
    }
}