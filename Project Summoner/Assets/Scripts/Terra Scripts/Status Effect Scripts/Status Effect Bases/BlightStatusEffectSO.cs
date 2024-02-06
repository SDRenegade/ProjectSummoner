using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Blight")]
public class BlightStatusEffectSO : StatusEffectSO
{
    public BlightStatusEffectSO() {}

    public override StatusEffectBase CreateStatusEffectInstance()
    {
        return new BlightStatusEffect(this);
    }
}

public class BlightStatusEffect : StatusEffectBase
{
    private static readonly float[] BLIGHT_DAMAGE_LIST = new float[5] {
        1/12f, 1/8f, 1/6f, 1/4f, 1/3f
    };

    private TerraBattlePosition terraBattlePosition;
    private int blightCounter;

    public BlightStatusEffect(StatusEffectSO statusEffectSO) : base(statusEffectSO)
    {
        blightCounter = 0;
    }

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnEndOfTurn += BlightActive;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= BlightActive;
    }

    private void BlightActive(object sender, BattleEventArgs eventArgs)
    {
        //Check that blightCounter is within a valid range
        if (blightCounter > BLIGHT_DAMAGE_LIST.Length)
            blightCounter = BLIGHT_DAMAGE_LIST.Length;
        else if (blightCounter < 0)
            blightCounter = 0;

        if (blightCounter < BLIGHT_DAMAGE_LIST.Length)
            blightCounter++;

        int blightDamage = (int)(terraBattlePosition.GetTerra().GetMaxHP() * BLIGHT_DAMAGE_LIST[blightCounter - 1]);
        Debug.Log(BattleDialog.BlightProkedMsg(terraBattlePosition.GetTerra(), blightDamage));
        eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, blightDamage);
    }
}
