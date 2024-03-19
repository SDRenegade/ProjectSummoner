using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Persim Berry")]
public class PersimBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new PersimBerry(this);
    }
}

public class PersimBerry : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public PersimBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostVolatileStatusEffectAdded += RemoveConfusion;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnPostVolatileStatusEffectAdded -= RemoveConfusion;
    }

    private void RemoveConfusion(object sender, VolatileStatusEffectEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (eventArgs.GetVolatileStatusEffect().GetVolatileStatusEffectSO().GetStatusName() != "Confusion")
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        terraBattlePosition.RemoveVolatileStatusEffect(eventArgs.GetVolatileStatusEffect().GetVolatileStatusEffectSO(), eventArgs.GetBattleSystem());

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}