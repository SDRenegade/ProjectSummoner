using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Lagging Tail")]
public class LaggingTailBase : ItemBase
{
    private TerraBattlePosition terraBattlePosition;

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        battleSystem.OnEnteringCombatState += SetAttackSpeedPriority;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnEnteringCombatState -= SetAttackSpeedPriority;
    }

    public void SetAttackSpeedPriority(object sender, BattleEventArgs eventArgs)
    {
        BattleSystem battleSystem = eventArgs.GetBattleSystem();
        foreach (TerraAttack terraAttack in battleSystem.GetBattleActionManager().GetTerraAttackList()) {
            if (terraAttack.GetAttackerPosition() == terraBattlePosition) {
                Debug.Log(BattleDialog.ItemProked(this));
                terraAttack.SetSpeedPriority(SpeedPriority.LOW);
            }
        }
    }
}
