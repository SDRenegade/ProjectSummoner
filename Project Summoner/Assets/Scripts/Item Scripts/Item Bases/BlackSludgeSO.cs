using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Black Sludge")]
public class BlackSludgeSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new BlackSludge(this);
    }
}

public class BlackSludge : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_UPDATE = 1/16f;
    private static readonly TerraType COMPATIBLE_TYPE = TerraType.BLIGHT;

    private TerraBattlePosition terraBattlePosition;

    public BlackSludge(ItemSO itemSO) : base(itemSO) {}

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
        int? hpUpdate = (int)(terra.GetMaxHP() * PERCENT_MAX_HEALTH_UPDATE);
        bool isCompatibleType = false;
        foreach(TerraType type in terra.GetTerraBase().GetTerraTypes()) {
            if(type == COMPATIBLE_TYPE) {
                isCompatibleType = true;
                break;
            }
        }

        if(isCompatibleType)
            eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, hpUpdate);
        else
            eventArgs.GetBattleSystem().DamageTerra(terraBattlePosition, hpUpdate);
    }
}
