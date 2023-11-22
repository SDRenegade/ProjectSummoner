using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield
{
    private BattleSide primaryBattleSide;
    private BattleSide secondaryBattleSide;

    private List<TerraAttack> terraAttackList;
    //private SummonerDie nextCatchAttemptDie;
    //private List<TerraSwitchBattleAction> TerraSwitchList;
    private bool attemptEscape;

    public Battlefield(TerraParty primarySideParty, Terra secondarySideTerra)
    {
        if (primarySideParty.GetTerraList()[0] != null)
            primaryBattleSide = new BattleSide(primarySideParty.GetTerraList()[0]);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        secondaryBattleSide = new BattleSide(secondarySideTerra);

        terraAttackList = new List<TerraAttack>();
        attemptEscape = false;
    }

    public Battlefield(TerraParty primarySideParty, TerraParty secondarySideParty)
    {
        if (primarySideParty.GetTerraList()[0] != null)
            primaryBattleSide = new BattleSide(primarySideParty.GetTerraList()[0]);
        else
            Debug.LogError("Primary Battle Side Party does not have a leading Terra");
        if (secondarySideParty.GetTerraList()[0] != null)
            secondaryBattleSide = new BattleSide(secondarySideParty.GetTerraList()[0]);
        else
            Debug.LogError("Secondary Battle Side Party does not have a leading Terra");

        terraAttackList = new List<TerraAttack>();
        attemptEscape = false;
    }

    public BattleSide GetPrimaryBattleSide() { return primaryBattleSide; }

    public BattleSide GetSecondaryBattleSide() { return secondaryBattleSide; }

    public List<Terra> GetAllTerraOnBattlefield()
    {
        return new List<Terra> {
            primaryBattleSide.GetTerraBattlePosition().GetTerra(),
            secondaryBattleSide.GetTerraBattlePosition().GetTerra()
        };
    }

    public List<TerraAttack> GetTerraAttackList() {  return terraAttackList; }

    public void SetTerraAttackList(List<TerraAttack> terraAttackList) { this.terraAttackList = terraAttackList; }

    public bool GetIsAttemptEscape() { return attemptEscape; }

    public void SetAttemptEscape(bool attemptEscape) { this.attemptEscape = attemptEscape; }

}
