using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Black Sludge")]
public class BlackSludgeBase : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_UPDATE = 1/16f;
    private static readonly TerraType COMPATIBLE_TYPE = TerraType.BLIGHT;

    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnEndOfTurn += TerraHealthUpdate;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEndOfTurn -= TerraHealthUpdate;
    }

    public void TerraHealthUpdate(object sender, BattleEventArgs eventArgs)
    {
        Debug.Log(BattleDialog.ItemProked(this));

        Terra terra = terraBattlePosition.GetTerra();
        int? hpUpdate = -(int)(terra.GetMaxHP() * PERCENT_MAX_HEALTH_UPDATE);
        foreach(TerraType type in terra.GetTerraBase().GetTerraTypes()) {
            if(type == COMPATIBLE_TYPE) {
                hpUpdate = -hpUpdate;
                break;
            }
        }
        eventArgs.GetBattleSystem().UpdateTerraHP(terraBattlePosition, hpUpdate);
    }
}
