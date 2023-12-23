using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager
{
    private List<TerraAttack> terraAttackList;
    //private SummonerDie nextCatchAttemptDie;
    //private List<TerraSwitchBattleAction> TerraSwitchList;
    private bool attemptEscape;

    public BattleActionManager()
    {
        terraAttackList = new List<TerraAttack>();
        attemptEscape = false;
    }

    public void ClearAllActions()
    {
        terraAttackList.Clear();
        attemptEscape = false;
    }

    public List<TerraAttack> GetTerraAttackList() { return terraAttackList; }

    public void SetTerraAttackList(List<TerraAttack> terraAttackList) { this.terraAttackList = terraAttackList; }

    public bool GetIsAttemptEscape() { return attemptEscape; }

    public void SetAttemptEscape(bool attemptEscape) { this.attemptEscape = attemptEscape; }
}
