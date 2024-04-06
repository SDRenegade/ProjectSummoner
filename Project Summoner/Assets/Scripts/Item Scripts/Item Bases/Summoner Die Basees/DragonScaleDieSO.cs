using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Summoner Die/Dragon Scale Die")]
public class DragonScaleDieSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new DragonScaleDie(this);
    }
}

public class DragonScaleDie : SummonerDieBase
{
    private static readonly float CAPTURE_MODIFIER = 1.5f;

    public DragonScaleDie(ItemSO itemSO) : base(itemSO) {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public override void OnOverworldUse() {}

    public override void RemoveItemListeners(BattleSystem battleSystem) {}

    public override float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem)
    {
        return CAPTURE_MODIFIER;
    }
}
