using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Ember")]
public class EmberSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Ember(terraAttack, this);
    }
}

public class Ember : TerraMoveBase
{
    private static readonly float BURN_CHANCE = 0.2f;

    public Ember(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        bool isBurned = BURN_CHANCE > Random.Range(0, 1f);
        if (!defenderPosition.GetTerra().HasStatusEffect() && isBurned) {
            Debug.Log(BattleDialog.BurnInflictedMsg(defenderPosition.GetTerra()));
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Burn"), defenderPosition, battleSystem);
        }
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}