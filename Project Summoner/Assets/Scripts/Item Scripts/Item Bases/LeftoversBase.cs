using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Leftovers")]
public class LeftoversBase : ItemBase
{
    private static readonly float PERCENT_HEALTH_HEAL = 1/16f;

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
        Terra terra = terraBattlePosition.GetTerra();
        int? healAmt = (int)(terra.GetMaxHP() * PERCENT_HEALTH_HEAL);
        healAmt = eventArgs.GetBattleSystem().UpdateTerraHP(terraBattlePosition, healAmt);
        if(healAmt != null)
            Debug.Log(BattleDialog.LeftoversMsg(terra, (int)healAmt));
    }
}