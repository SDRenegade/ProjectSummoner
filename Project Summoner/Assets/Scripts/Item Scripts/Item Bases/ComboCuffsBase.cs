using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Combo Cuffs")]
public class ComboCuffsBase : ItemBase
{
    private static readonly float[] DAMAGE_MODIFIER_LIST = new float[5] {
        1f, 1.25f, 1.5f, 1.75f, 2f
    };

    private TerraBattlePosition terraBattlePosition;
    private TerraMoveBase previousAttack;
    private int comboCounter;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        previousAttack = null;
        comboCounter = 0;
        battleSystem.OnDirectAttack += AddDamageModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= AddDamageModifier;
    }

    public void AddDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveBase().GetMoveName() == previousAttack?.GetMoveName()) {
            if(comboCounter < DAMAGE_MODIFIER_LIST.Length - 1)
                comboCounter++;
        }
        else {
            comboCounter = 0;
            previousAttack = eventArgs.GetDirectAttackParams().GetMove().GetMoveBase();
        }

        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER_LIST[comboCounter]);
    }
}
