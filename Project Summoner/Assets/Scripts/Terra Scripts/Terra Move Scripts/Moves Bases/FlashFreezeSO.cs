using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Flash Freeze")]
public class FlashFreezeSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new FlashFreeze(terraAttack, this);
    }
}

public class FlashFreeze : TerraMoveBase
{
    private static readonly float FREEZE_CHANCE = 0.2f;

    public FlashFreeze(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem)
    {
        TerraBattlePosition defenderPosition = directAttackLog.GetDefenderPosition();

        if(FREEZE_CHANCE >= Random.Range(0f, 1f))
            battleSystem.AddStatusEffect(defenderPosition, SODatabase.GetInstance().GetStatusEffectByName("Freeze"));
    }

    public override void AddMoveListeners(BattleSystem battleSystem) {}

    public override void RemoveMoveListeners(BattleSystem battleSystem) {}
}