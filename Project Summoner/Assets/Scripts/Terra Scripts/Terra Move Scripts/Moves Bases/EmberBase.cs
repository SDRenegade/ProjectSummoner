using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Ember")]
public class EmberBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new EmberAction();
    }
}

public class EmberAction : TerraMoveAction
{
    private static readonly float BURN_CHANCE = 0.2f;

    public EmberAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        bool isBurned = BURN_CHANCE > Random.Range(0, 1f);
        if (!defenderPosition.GetTerra().HasStatusEffect() && isBurned) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Burn"), defenderPosition, battleSystem);
            Debug.Log(BattleDialog.BurnInflictedMsg(defenderPosition.GetTerra()));
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}