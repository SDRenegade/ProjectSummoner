using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Metronome")]
public class MetronomeBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new MetronomeAction(terraAttack);
    }
}

public class MetronomeAction : TerraMoveAction
{
    private TerraAttack terraAttack;

    public MetronomeAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += PickRandomMove;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= PickRandomMove;
    }

    public void PickRandomMove(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack() != terraAttack)
            return;

        TerraMove randomMove = new TerraMove(SODatabase.GetInstance().GetRandomMetronomeMove());
        terraAttack.SetMove(randomMove);
        terraAttack.GetTerraMoveAction()?.AddBattleActions(eventArgs.GetBattleSystem());
        Debug.Log(BattleDialog.MetronomeMoveMsg(randomMove));

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}