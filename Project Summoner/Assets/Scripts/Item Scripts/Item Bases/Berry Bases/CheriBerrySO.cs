using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Cheri Berry")]
public class CheriBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new CheriBerry(this);
    }
}

public class CheriBerry : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public CheriBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostStatusEffectAdded += RemoveParalysis;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostStatusEffectAdded -= RemoveParalysis;
    }

    private void RemoveParalysis(object sender, StatusEffectEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (eventArgs.GetStatusEffectSO().GetStatusName() != "Paralysis")
            return;

        terraBattlePosition.GetTerra().SetStatusEffect(null);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}