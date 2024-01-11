using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Protect")]
public class ProtectBase : TerraMoveBase
{
    public override TerraMoveAction CreateTerraMoveAction(TerraAttack terraAttack)
    {
        return new ProtectAction(terraAttack);
    }
}

public class ProtectAction : TerraMoveAction
{
    private TerraAttack terraAttack;
    private bool isCooldown;

    public ProtectAction(TerraAttack terraAttack)
    {
        this.terraAttack = terraAttack;
        isCooldown = false;
    }

    public void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem) {}

    public void AddBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack += ActivateProtect;
        battleSystem.OnEndOfTurn += ActiveCooldownIncrement;
    }

    public void RemoveBattleActions(BattleSystem battleSystem)
    {
        battleSystem.OnDirectAttack -= ActivateProtect;
        battleSystem.OnAttackDeclaration -= ActiveCooldown;
        battleSystem.OnEndOfTurn -= ActiveCooldownIncrement;
    }

    private void ActivateProtect(object sender, DirectAttackEventArgs eventArgs)
    {
        if (eventArgs.GetDirectAttackParams().GetDefenderPosition().GetTerra() != terraAttack.GetAttackerPosition().GetTerra())
            return;

        eventArgs.SetCanceled(true);
        Debug.Log(BattleDialog.ProtectActiveMsg(terraAttack.GetAttackerPosition().GetTerra()));

        eventArgs.GetBattleSystem().OnAttackDeclaration += ActiveCooldown;
        eventArgs.GetBattleSystem().OnDirectAttack -= ActivateProtect;
    }

    private void ActiveCooldown(object sender, AttackDeclarationEventArgs eventArgs)
    {
        if (eventArgs.GetTerraAttack().GetAttackerPosition().GetTerra() != terraAttack.GetAttackerPosition().GetTerra())
            return;
        if (eventArgs.GetTerraAttack().GetMove().GetMoveBase() != terraAttack.GetMove().GetMoveBase())
            return;
        if (!isCooldown)
            return;

        eventArgs.GetTerraAttack().SetCanceled(true);
        Debug.Log(BattleDialog.ProtectCooldownMsg(eventArgs.GetTerraAttack().GetAttackerPosition().GetTerra()));
    }

    private void ActiveCooldownIncrement(object sender, BattleEventArgs eventArgs)
    {
        if (!isCooldown)
            isCooldown = true;
        else
            RemoveBattleActions(eventArgs.GetBattleSystem());
    }
}