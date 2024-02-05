using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/Berry/Jaboca Berry")]
public class JabocaBerrySO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new JabocaBerry(this);
    }
}

public class JabocaBerry : ItemBase
{
    private static readonly float PERCENT_MAX_HEALTH_DAMAGE = 1/8f;

    private TerraBattlePosition terraBattlePosition;

    public JabocaBerry(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnPostTerraDamageByTerra += PhysicalAttackDamageRecoil;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnPostTerraDamageByTerra -= PhysicalAttackDamageRecoil;
    }

    private void PhysicalAttackDamageRecoil(object sender, TerraDamageByTerraEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackLog().GetDefenderPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackLog().GetDirectAttackParams().GetMove().GetMoveSO().GetDamageType() != DamageType.PHYSICAL)
            return;
        if (eventArgs.GetDirectAttackLog().GetDamage() == null)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        int damage = (int)(eventArgs.GetDirectAttackLog().GetAttackerPosition().GetTerra().GetMaxHP() * PERCENT_MAX_HEALTH_DAMAGE);
        eventArgs.GetBattleSystem().DamageTerra(eventArgs.GetDirectAttackLog().GetAttackerPosition(), damage);

        ConsumeOnUse(terraBattlePosition, eventArgs.GetBattleSystem());
    }
}