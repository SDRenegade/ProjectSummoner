using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Quick Claw")]
public class QuickClawSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new QuickClaw(this);
    }
}

public class QuickClaw : ItemBase
{
    private static readonly float SPEED_PRIORITY_CHANCE = 1/5f;

    private TerraBattlePosition terraBattlePosition;

    public QuickClaw(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnEnteringCombatState += SetSpeedPriority;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState -= SetSpeedPriority;
    }

    public void SetSpeedPriority(object sender, BattleEventArgs eventArgs)
    {
        if (Random.Range(0f, 1f) > SPEED_PRIORITY_CHANCE)
            return;

        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == terraBattlePosition) {
                Debug.Log(BattleDialog.ItemProked(this));
                terraAttack.SetSpeedPriority(SpeedPriority.HIGH);
            }
        }

    }
}
