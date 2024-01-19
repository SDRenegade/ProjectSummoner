using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dream Eater")]
public class DreamEaterBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DreamEaterAction(terraAttack);
    }
}

public class DreamEaterAction : TerraMoveAction
{
    TerraAttack terraAttack;

    public DreamEaterAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        if (directAttackLog.GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)directAttackLog.GetDamage() / 2f);
            battleSystem.UpdateTerraHP(attackerPosition, healthAbsorbed);
        }
    }

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += CheckOpponentIsSleeping;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= CheckOpponentIsSleeping;
    }

    private void CheckOpponentIsSleeping(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetMove() != terraAttack.GetMove())
            return;

        Terra defendingTerra = eventArgs.GetDirectAttackParams().GetDefenderPosition().GetTerra();
        if (defendingTerra.GetStatusEffectWrapper().GetStatusEffectBase() != SODatabase.GetInstance().GetStatusEffectByName("Sleep"))
            eventArgs.SetCanceled(true);
    }
}
