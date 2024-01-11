using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Decay")]
public class DecayBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DecayAction();
    }
}

public class DecayAction : TerraMoveAction
{
    public DecayAction() {}

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        if (!defenderPosition.GetTerra().HasStatusEffect()) {
            defenderPosition.GetTerra().SetStatusEffect(SODatabase.GetInstance().GetStatusEffectByName("Blight"), battleSystem);
            Debug.Log(BattleDialog.BlightInflictedMsg(defenderPosition.GetTerra()));
        }
    }

    public void AddBattleActions(BattleSystem battleSystem) {}

    public void RemoveBattleActions(BattleSystem battleSystem) {}
}