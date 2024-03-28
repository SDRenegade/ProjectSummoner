using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Iron Ball")]
public class IronBallSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new IronBall(this);
    }
}

//TODO Add functionallity that grounds a terra when they have holding an item or has an
//ability that gives them levitate
public class IronBall : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public IronBall(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddItemListeners(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;

        battleSystem.OnEnteringCombatState += SetSpeedPriority;
        battleSystem.OnDirectAttack += GroundFlyingTerra;
    }

    public override void RemoveItemListeners(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState -= SetSpeedPriority;
        battleSystem.OnDirectAttack -= GroundFlyingTerra;
    }

    private void SetSpeedPriority(object sneder, BattleEventArgs eventArgs)
    {
        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == terraBattlePosition) {
                Debug.Log(BattleDialog.ItemProkedMsg(this));
                terraAttack.SetSpeedPriority(SpeedPriority.LOW);
            }
        }
    }

    private void GroundFlyingTerra(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition() != terraBattlePosition)
            return;
        if (eventArgs.GetDirectAttackParams().GetMove().GetMoveSO().GetTerraType() != TerraType.GAIA)
            return;

        for (int i = 0; i < eventArgs.GetDirectAttackParams().GetDefenderTerraTypeList().Count; i++) {
            if (eventArgs.GetDirectAttackParams().GetDefenderTerraTypeList()[i] == TerraType.AVIAN) {
                Debug.Log(BattleDialog.ItemProkedMsg(this));
                eventArgs.GetDirectAttackParams().GetDefenderTerraTypeList()[i] = TerraType.TYPELESS;
            }
        }
    }
}