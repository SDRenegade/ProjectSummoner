using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VolatileStatusEffect", menuName = "VolatileStatusEffect/Bound")]
public class BoundVolatileStatusEffectSO : VolatileStatusEffectSO
{
    public override VolatileStatusEffectBase CreateVolatileStatusEffect(TerraBattlePosition terraBattlePosition)
    {
        return new BoundVolatileStatusEffect(terraBattlePosition, this);
    }
}

public class BoundVolatileStatusEffect : VolatileStatusEffectBase
{
    private readonly float PERCENT_HEALTH_DAMAGE = 1/16f;
    private readonly int MIN_TURN_DURATION = 2;
    private readonly int MAX_TURN_DURATION = 5;

    private int turnDuration;
    private int turnCounter;

    public BoundVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO) : base(terraBattlePosition, vStatusEffectSO)
    {
        turnDuration = Random.Range(MIN_TURN_DURATION, MAX_TURN_DURATION + 1);
        turnCounter = 0;
    }

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn += EndOfTurnDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= EndOfTurnDamage;
    }

    private void EndOfTurnDamage(object sender, BattleEventArgs eventArgs)
    {
        Terra defendingTerra = terraBattlePosition.GetTerra();
        int boundDamage = (int)(defendingTerra.GetMaxHP() * PERCENT_HEALTH_DAMAGE);

        Debug.Log(BattleDialog.BindDamageMsg(defendingTerra, boundDamage));
        eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, boundDamage);

        turnCounter++;
        if (turnCounter >= turnDuration)
            terraBattlePosition.RemoveVolatileStatusEffect(vStatusEffectSO, eventArgs.GetBattleSystem());
    }
}