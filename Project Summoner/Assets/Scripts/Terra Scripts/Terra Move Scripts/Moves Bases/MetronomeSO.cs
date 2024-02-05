using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Metronome")]
public class MetronomeSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new Metronome(terraAttack, this);
    }
}

public class Metronome : TerraMoveBase
{
    public Metronome(TerraAttack terraAttack, TerraMoveSO terraMoveSO) : base(terraAttack, terraMoveSO) {}

    public override void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public override void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration += PickRandomMove;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= PickRandomMove;
    }

    public void PickRandomMove(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack() != terraAttack)
            return;

        TerraMove randomMove = new TerraMove(SODatabase.GetInstance().GetRandomMetronomeMove());
        terraAttack.SetMove(randomMove);
        terraAttack.GetTerraMoveBase()?.AddBattleActions(eventArgs.GetBattleSystem());
        Debug.Log(BattleDialog.MetronomeMoveMsg(randomMove));

        RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}