using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Leftovers")]
public class LeftoversBase : ItemBase
{
    private static readonly float PERCENT_HEALTH_HEAL = 1/2f;

    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnEndOfTurn += HealthRegen;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= HealthRegen;
    }

    public void HealthRegen(object sender, BattleEventArgs eventArgs)
    {
        Debug.Log(BattleDialog.ItemProked(this));

        Terra terra = terraBattlePosition.GetTerra();
        int? healAmt = (int)(terra.GetMaxHP() * PERCENT_HEALTH_HEAL);
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, healAmt);
    }
}