using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Summoner Die/Dragon Scale Summoner Die")]
public class DragonScaleSummonerDieSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new DragonScaleSummonerDie(this);
    }
}

public class DragonScaleSummonerDie : SummonerDieBase
{
    private static readonly float CAPTURE_MODIFIER = 1.5f;

    public DragonScaleSummonerDie(ItemSO itemSO) : base(itemSO) {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem) {}

    public override void OnOverworldUse() {}

    public override void RemoveItemListeners(BattleSystem battleSystem) {}

    public override float? GetCaptureModifier(CaptureAttempt captureAttempt, BattleSystem battleSystem)
    {
        return CAPTURE_MODIFIER;
    }
}
