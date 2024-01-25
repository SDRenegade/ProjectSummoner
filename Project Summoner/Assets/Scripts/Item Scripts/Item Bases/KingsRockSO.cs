using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/King's Rock")]
public class KingsRockSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new KingsRock(this);
    }
}

public class KingsRock : ItemBase
{
    private static readonly float FLINCH_CHANCE = 1/5f;

    private TerraBattlePosition terraBattlePosition;

    public KingsRock(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnDirectAttack += InflictFlinch;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= InflictFlinch;
    }

    public void InflictFlinch(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (Random.Range(0f, 1f) > FLINCH_CHANCE)
            return;

        Debug.Log(BattleDialog.ItemProked(this));
        eventArgs.GetBattleSystem().AddVolatileStatusEffect(terraBattlePosition, SODatabase.GetInstance().GetVolatileStatusEffectByName("Flinched"));
    }
}
