using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlightStatusEffect : StatusEffect
{
    private static readonly float[] BLIGHT_DAMAGE_LIST = new float[5] {
        1/12f, 1/8f, 1/6f, 1/4f, 1/3f
    };

    [SerializeField] private int blightCounter;

    public BlightStatusEffect(Terra terra) : base(terra)
    {
        blightCounter = 0;
    }

    public override void AddBattleEvent(BattleSystem battleSystem)
    {
        blightCounter = 0;
        battleSystem.OnEndTurn += BlightActive;
        //Add AttackReductionActive
    }

    public override void RemoveBattleEvent(BattleSystem battleSystem)
    {
        blightCounter = 0;
        battleSystem.OnEndTurn -= BlightActive;
        //Remove AttackReductionActive
    }

    private void BlightActive(object sender, BattleEventArgs battlesArgs)
    {
        //Check that blightCounter is within a valid range
        if(blightCounter > BLIGHT_DAMAGE_LIST.Length)
            blightCounter = BLIGHT_DAMAGE_LIST.Length;
        else if(blightCounter < 0)
            blightCounter = 0;

        if(blightCounter < BLIGHT_DAMAGE_LIST.Length)
            blightCounter++;

        int blightDamage = (int)(terra.GetMaxHP() * BLIGHT_DAMAGE_LIST[blightCounter - 1]);
        terra.TakeDamage(blightDamage);
        Debug.Log(BattleDialog.BlightProkedMsg(terra, blightDamage));
        battlesArgs.GetBattleSystem().UpdateTerraStatusBars();
    }
}
