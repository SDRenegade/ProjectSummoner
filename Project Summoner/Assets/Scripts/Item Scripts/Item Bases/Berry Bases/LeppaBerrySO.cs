using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Leppa Berry")]
public class LeppaBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LeppaBerry(this);
    }
}

public class LeppaBerry : ItemBase
{
    private static readonly int PP_RECOVERED = 10;

    private TerraBattlePosition terraBattlePosition;

    public LeppaBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostAttack += RecoverPP;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnPostAttack -= RecoverPP;
    }

    private void RecoverPP(object sender, DirectAttackLogEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetAttackerPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove().GetCurrentPP() > 0)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove().RecoverPP(PP_RECOVERED);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}