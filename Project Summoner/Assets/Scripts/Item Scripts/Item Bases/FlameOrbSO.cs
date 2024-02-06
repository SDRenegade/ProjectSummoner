using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Flame Orb")]
public class FlameOrbSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new FlameOrb(this);
    }
}

public class FlameOrb : ItemBase
{
    private TerraBattlePosition terraBattlePosition;
    private bool isFirstInteraction;

    public FlameOrb(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        isFirstInteraction = true;
        battleSystem.OnStartOfTurn += InflictBurn;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnStartOfTurn -= InflictBurn;
    }

    public void InflictBurn(object sender, BattleEventArgs eventArgs)
    {
        if (!isFirstInteraction)
            return;

        isFirstInteraction = false;
        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().AddStatusEffect(terraBattlePosition, SODatabase.GetInstance().GetStatusEffectByName("Burn"));
    }
}
