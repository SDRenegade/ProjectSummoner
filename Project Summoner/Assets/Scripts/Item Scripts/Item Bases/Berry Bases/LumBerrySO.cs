using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Lum Berry")]
public class LumBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LumBerry(this);
    }
}

public class LumBerry : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public LumBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostStatusEffectAdded += RemoveStatusEffect;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostStatusEffectAdded -= RemoveStatusEffect;
    }

    private void RemoveStatusEffect(object sender, StatusEffectEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        terraBattlePosition.GetTerra().SetStatusEffect(null, terraBattlePosition, eventArgs.GetBattleSystem());

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}