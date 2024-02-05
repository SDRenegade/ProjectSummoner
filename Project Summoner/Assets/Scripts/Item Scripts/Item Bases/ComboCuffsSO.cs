using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Combo Cuffs")]
public class ComboCuffsSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new ComboCuffs(this);
    }
}

public class ComboCuffs : ItemBase
{
    private static readonly float[] DAMAGE_MODIFIER_LIST = new float[5] {
        1f, 1.25f, 1.5f, 1.75f, 2f
    };

    private TerraBattlePosition terraBattlePosition;
    private TerraMoveSO previousAttack;
    private int comboCounter;

    public ComboCuffs(ItemSO itemSO) : base(itemSO) {}

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

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveSO().GetMoveName() == previousAttack?.GetMoveName()) {
            if(comboCounter < DAMAGE_MODIFIER_LIST.Length - 1)
                comboCounter++;
        }
        else {
            comboCounter = 0;
            previousAttack = eventArgs.GetDirectAttackParams().GetMove().GetMoveSO();
        }

        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER_LIST[comboCounter]);
    }
}
