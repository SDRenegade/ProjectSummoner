using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffect/Blight")]
public class BlightStatusEffect : StatusEffectBase
{
    public BlightStatusEffect()
    {
        statusName = "Blight";
        description = "Inflicted Terra take increased amounts of damage each turn";
    }

    public override BattleAction CreateBattleAction(Terra terra)
    {
        return new BlightStatusEffectBattleAction(terra);
    }
}

public class BlightStatusEffectBattleAction : BattleAction
{
    private static readonly float[] BLIGHT_DAMAGE_LIST = new float[5] {
        1/12f, 1/8f, 1/6f, 1/4f, 1/3f
    };

    private Terra terra;
    private int blightCounter;

    public BlightStatusEffectBattleAction(Terra terra)
    {
        this.terra = terra;
        blightCounter = 0;
    }

    public void AddBattleAction(BattleSystem battleSystem)
    {
        blightCounter = 0;
        battleSystem.OnEnteringEndTurnState += BlightActive;
    }

    public void RemoveBattleAction(BattleSystem battleSystem)
    {
        blightCounter = 0;
        battleSystem.OnEnteringEndTurnState -= BlightActive;
    }

    private void BlightActive(object sender, BattleEventArgs battlesArgs)
    {
        //Check that blightCounter is within a valid range
        if (blightCounter > BLIGHT_DAMAGE_LIST.Length)
            blightCounter = BLIGHT_DAMAGE_LIST.Length;
        else if (blightCounter < 0)
            blightCounter = 0;

        if (blightCounter < BLIGHT_DAMAGE_LIST.Length)
            blightCounter++;

        int blightDamage = (int)(terra.GetMaxHP() * BLIGHT_DAMAGE_LIST[blightCounter - 1]);
        terra.TakeDamage(blightDamage);
        Debug.Log(BattleDialog.BlightProkedMsg(terra, blightDamage));
        battlesArgs.GetBattleSystem().UpdateTerraStatusBars();
    }
}
