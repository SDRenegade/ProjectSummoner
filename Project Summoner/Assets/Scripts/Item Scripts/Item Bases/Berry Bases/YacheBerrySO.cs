using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Yache Berry")]
public class YacheBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new YacheBerry(this);
    }
}

public class YacheBerry : ItemBase
{
    private static readonly float DAMAGE_REDUCTION_MODIFIER = 0.5f;

    private TerraBattlePosition terraBattlePosition;

    public YacheBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnDirectAttack += SuperEffectiveMoveDamageReduction;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= SuperEffectiveMoveDamageReduction;
    }

    private void SuperEffectiveMoveDamageReduction(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;
        TerraMove move = eventArgs.GetDirectAttackParams().GetMove();
        List<TerraType> defenderTypes = eventArgs.GetDirectAttackParams().GetDefenderTerraTypeList();
        if (move.GetMoveSO().GetTerraType().GetTypeEffectiveness(defenderTypes) != EffectivenessTypes.SUPER)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_REDUCTION_MODIFIER);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}