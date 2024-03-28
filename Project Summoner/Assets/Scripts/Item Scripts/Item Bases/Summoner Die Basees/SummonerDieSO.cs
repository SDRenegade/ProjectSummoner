using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Summoner Die/Summoner Die")]
public class SummonerDieSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new SummonerDie(this);
    }
}

public class SummonerDie : SummonerDieBase
{
    private static readonly float CAPTURE_MODIFIER = 1.0f;

    public SummonerDie(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public override void RemoveItemListeners(BattleSystem battleSystem) {}

    public override float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem)
    {
        return CAPTURE_MODIFIER;
    }
}
