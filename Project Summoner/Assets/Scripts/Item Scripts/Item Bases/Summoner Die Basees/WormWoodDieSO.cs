using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Summoner Die/Worm Wood Die")]
public class WormWoodDieSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new WormWoodDie(this);
    }
}

public class WormWoodDie : SummonerDieBase
{
    private static readonly float CAPTURE_MODIFIER = 1.25f;

    public WormWoodDie(ItemSO itemSO) : base(itemSO) {}
    
    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public override void RemoveItemListeners(BattleSystem battleSystem) {}

    public override float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem)
    {
        return CAPTURE_MODIFIER;
    }
}
