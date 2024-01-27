using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

[CreateAssetMenu(fileName = "ItemBase", menuName = "Item/Choice Band")]
public class ChoiceBandSO : ItemSO
{
    public override ItemBase CreateItemBase()
    {
        return new ChoiceBand(this);
    }
}

public class ChoiceBand : ItemBase
{
    private static readonly float DAMAGE_MODIFIER = 1.5f;

    private TerraBattlePosition terraBattlePosition;
    private int? chosenMoveIndex;
    private List<TerraBattlePosition> defenderList;

    public ChoiceBand(ItemSO itemSO) : base(itemSO) {}

    public override void OnOverworldUse() {}

    public override void AddBattleActions(TerraBattlePosition terraBattlePosition, BattleSystem battleSystem)
    {
        this.terraBattlePosition = terraBattlePosition;
        chosenMoveIndex = null;
        defenderList = null;

        battleSystem.OnAttackDeclaration += SetChosenAttack;
        battleSystem.OnOpeningMoveSelectionUI += RepeatAttack;
        battleSystem.OnDirectAttack += ApplyDamageModifier;
    }

    public override void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnAttackDeclaration -= SetChosenAttack;
        battleSystem.OnOpeningMoveSelectionUI -= RepeatAttack;
        battleSystem.OnDirectAttack -= ApplyDamageModifier;
    }

    public void SetChosenAttack(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition() != terraBattlePosition)
            return;
        if (chosenMoveIndex != null)
            return;

        for(int i = 0; i < terraBattlePosition.GetTerra().GetMoves().Count - 1; i++) {
            if (terraBattlePosition.GetTerra().GetMoves()[i].ToString() == eventArgs.GetTerraAttack().GetMove().ToString()) {
                chosenMoveIndex = i;
                defenderList = eventArgs.GetTerraAttack().GetDefendersPositionList();
                break;
            }
        }
    }

    private void RepeatAttack(object sender, OpeningMoveSelectionUIEventArgs eventArgs)
    {
        if (eventArgs.GetTerraBattlePosition() != terraBattlePosition)
            return;
        if (chosenMoveIndex == null || defenderList == null)
            return;
        if (terraBattlePosition.GetTerra().GetMoves()[(int)chosenMoveIndex].GetCurrentPP() <= 0)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetBattleSystem().MoveSelectionAction(terraBattlePosition, defenderList, (int)chosenMoveIndex);
        eventArgs.SetMoveSelectionCancled(true);
    }

    public void ApplyDamageModifier(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetAttackerPosition() != terraBattlePosition)
            return;
        if (chosenMoveIndex == null)
            return;

        Debug.Log(BattleDialog.ItemProkedMsg(this));
        eventArgs.GetDirectAttackParams().AddDamageModifier(DAMAGE_MODIFIER);
    }
}