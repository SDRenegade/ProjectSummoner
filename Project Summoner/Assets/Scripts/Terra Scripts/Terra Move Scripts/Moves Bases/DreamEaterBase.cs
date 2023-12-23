using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Dream Eater")]
public class DreamEaterBase : TerraMoveBase
{
    public override void AttackSelectionInit(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        new DreamEaterAction(terraAttack, battleSystem);
    }

    public override void PreAttackEffect(TerraAttackParams terraAttackParams, BattleSystem battleSystem) {}

    public override void PostAttackEffect(List<TerraAttackLog> terraAttackLogList, BattleSystem battleSystem)
    {
        Terra attacker = terraAttackLogList[0].GetAttackerPosition().GetTerra();

        if (terraAttackLogList[0].GetDamage() != null) {
            int healthAbsorbed = (int)Mathf.Ceil((int)terraAttackLogList[0].GetDamage() / 2f);
            attacker.SetCurrentHP(attacker.GetCurrentHP() + healthAbsorbed);
            Debug.Log(BattleDialog.HealthHealedMsg(attacker, healthAbsorbed));
        }
    }
}

public class DreamEaterAction
{
    TerraAttack thisTerraAttack;

    public DreamEaterAction(TerraAttack terraAttack, BattleSystem battleSystem)
    {
        thisTerraAttack = terraAttack;
        battleSystem.OnTerraAttackDeclaration += CheckOpponentIsSleeping;
    }

    private void CheckOpponentIsSleeping(object sender, TerraAttackDeclarationEventArgs eventArgs)
    {
        if (thisTerraAttack != eventArgs.GetTerraAttack())
            return;

        TerraBattlePosition defenderPosition = eventArgs.GetTerraAttack().GetDefendersPositionList()[0];

        if (defenderPosition.GetTerra().GetStatusEffect().GetStatusEffectBase() != SODatabase.GetInstance().GetStatusEffectByName("Sleep"))
            thisTerraAttack.SetIsCanceled(true);
    }
}
