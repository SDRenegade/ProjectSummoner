using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Decay")]
public class DecaySO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Decay(terraAttack, this);
    }
}

public class Decay : TerraMoveBase
{
    public Decay(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        if (!defenderPosition.GetTerra().HasStatusEffect()) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Blight"), defenderPosition, battleSystem);
            Debug.Log(BattleDialog.BlightInflictedMsg(defenderPosition.GetTerra()));
        }
    }

    public override void AddBattleActions(BattleSystem battleSystem) {}

    public override void RemoveBattleActions(BattleSystem battleSystem) {}
}