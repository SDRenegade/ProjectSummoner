using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Light Clay")]
public class LightClaySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new LightClay(this);
    }
}

public class LightClay : ItemBase
{
    private static readonly int EXTENDED_TURN_DURATION = 8;

    private TerraBattlePosition terraBattlePosition;

    public LightClay(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnVolatileStatusEffectAdded += EnhanceBarrierEffect;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnVolatileStatusEffectAdded -= EnhanceBarrierEffect;
    }

    public void EnhanceBarrierEffect(object sender, VolatileStatusEffectAddedEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (eventArgs.GetVolatileStatusEffect().GetVolatileStatusEffectSO().GetStatusName() != "Special Barrier")
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        SpecialBarrierVolatileStatusEffect barrierEffect = (SpecialBarrierVolatileStatusEffect)eventArgs.GetVolatileStatusEffect();
        barrierEffect.SetTurnDuration(EXTENDED_TURN_DURATION);
    }
}
