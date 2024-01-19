using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Blight")]
public class BlightStatusEffect : StatusEffectBase
{
    public BlightStatusEffect() {}

    public override BattleAction CreateBattleAction(TerraBattlePosition terraBattlePosition)
    {
        return new BlightStatusEffectAction(terraBattlePosition);
    }
}

public class BlightStatusEffectAction : BattleAction
{
    private static readonly float[] BLIGHT_DAMAGE_LIST = new float[5] {
        1/12f, 1/8f, 1/6f, 1/4f, 1/3f
    };

    private TerraBattlePosition terraBattlePosition;
    private int blightCounter;

    public BlightStatusEffectAction(TerraBattlePosition terraBattlePosition)
    {
        this.terraBattlePosition = terraBattlePosition;
        blightCounter = 0;
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        blightCounter = 0;
        battleSystem.OnEndOfTurn += BlightActive;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        blightCounter = 0;
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
        eventArgs.GetBattleSystem().UpdateTerraHP(terraBattlePosition, blightDamage);
    }
}
