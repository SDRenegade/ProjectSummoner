using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dream Eater")]
public class DreamEaterSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new DreamEater(terraAttack, this);
    }
}

public class DreamEater : TerraMoveBase
{
    public DreamEater(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition attackerPosition = directAttackLog.GetAttackerPosition();

        if (directAttackLog.GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)directAttackLog.GetDamage() / 2f);
            battleSystem.HealTerra(attackerPosition, healthAbsorbed);
        }
    }

    public override void AddMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += CheckOpponentIsSleeping;
    }

    public override void RemoveMoveListeners(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= CheckOpponentIsSleeping;
    }

    private void CheckOpponentIsSleeping(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetMove() != terraAttack.GetMove())
            return;

        Terra defendingTerra = eventArgs.GetDirectAttackParams().GetDefenderPosition().GetTerra();
        if (defendingTerra.GetStatusEffect().GetStatusEffectSO() != SODatabase.GetInstance().GetStatusEffectByName("Sleep"))
            eventArgs.SetCanceled(true);
    }
}
