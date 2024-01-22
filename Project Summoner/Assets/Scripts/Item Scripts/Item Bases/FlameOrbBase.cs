using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Flame Orb")]
public class FlameOrbBase : ItemBase
{
    private TerraBattlePosition terraBattlePosition;
    private bool isFirstInteraction;

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
        Debug.Log(BattleDialog.ItemProked(this));
        if (!terraBattlePosition.GetTerra().HasStatusEffect()) {
            Debug.Log(BattleDialog.BurnInflictedMsg(terraBattlePosition.GetTerra()));
            terraBattlePosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Burn"), terraBattlePosition, eventArgs.GetBattleSystem());
        }
    }
}
