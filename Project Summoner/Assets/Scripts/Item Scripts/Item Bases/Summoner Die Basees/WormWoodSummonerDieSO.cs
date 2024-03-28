using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Summoner Die/Worm Wood Summoner Die")]
public class WormWoodSummonerDieSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new WormWoodSummonerDie(this);
    }
}

public class WormWoodSummonerDie : SummonerDieBase
{
    private static readonly float CAPTURE_MODIFIER = 1.25f;

    public WormWoodSummonerDie(ItemSO itemSO) : base(itemSO) {}
    
    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public override void RemoveItemListeners(BattleSystem battleSystem) {}

    public override float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem)
    {
        return CAPTURE_MODIFIER;
    }
}
