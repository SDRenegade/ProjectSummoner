using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Bound")]
public class BoundVolatileStatusEffect : VolatileStatusEffectBase
{
    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new BoundVolatileStatusEffectAction(terraBattlePosition, this);
    }
}

public class BoundVolatileStatusEffectAction : BattleAction
{
    private readonly float PERCENT_HEALTH_DAMAGE = 1/16f;
    private readonly int MIN_TURN_DURATION = 2;
    private readonly int MAX_TURN_DURATION = 5;

    private TerraBattlePosition terraBattlePosition;
    private VolatileStatusEffectBase vStatusEffect;
    private int turnDuration;
    private int turnCounter;

    public BoundVolatileStatusEffectAction(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        this.terraBattlePosition = terraBattlePosition;
        this.vStatusEffect = vStatusEffect;
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn += EndOfTurnDamage;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= EndOfTurnDamage;
        terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffect);
    }

    private void EndOfTurnDamage(object sender, BattleEventArgs eventArgs)
    {
        Terra defendingTerra = terraBattlePosition.GetTerra();
        int boundDamage = (int)(defendingTerra.GetMaxHP() * PERCENT_HEALTH_DAMAGE);

        Debug.Log(BattleDialog.BindDamageMsg(defendingTerra, boundDamage));
        eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, boundDamage);

        turnCounter++;
        if (turnCounter >= turnDuration) {
            RemoveBattleActions(eventArgs.GetBattleSystem());
            terraBattlePosition.RemoveVolatileStatusEffect(SODatabase.GetInstance().GetVolatileStatusEffectByName("Bound"));
        }
    }
}
