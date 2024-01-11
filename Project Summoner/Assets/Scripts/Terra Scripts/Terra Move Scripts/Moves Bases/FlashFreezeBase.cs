using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Flash Freeze")]
public class FlashFreezeBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new FlashFreezeAction();
    }
}

public class FlashFreezeAction : TerraMoveAction
{
    private static readonly float FREEZE_CHANCE = 0.2f;

    public FlashFreezeAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        bool hasFreezeProked = FREEZE_CHANCE >= Random.Range(0f, 1f);
        if (!defenderPosition.GetTerra().HasStatusEffect() && hasFreezeProked) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Freeze"), battleSystem);
            BattleDialog.FreezeInflictedMsg(defenderPosition.GetTerra());
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}