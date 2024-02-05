using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Enigma Berry")]
public class EnigmaBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new EnigmaBerry(this);
    }
}

public class EnigmaBerry : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_RECOVER = 1/8f;

    private TerraBattlePosition terraBattlePosition;

    public EnigmaBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += RecoverAfterSuperEffectiveMoveDamage;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= RecoverAfterSuperEffectiveMoveDamage;
    }

    private void RecoverAfterSuperEffectiveMoveDamage(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        TerraMove move = eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove();
        List<TerraType> defenderTypes = eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetDefenderTerraTypeList();
        if (move.GetMoveSO().GetTerraType().GetTypeEffectiveness(defenderTypes) != EffectivenessTypes.SUPER)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().HealTerra(terraBattlePosition, (int)(terraBattlePosition.GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_RECOVER));

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}