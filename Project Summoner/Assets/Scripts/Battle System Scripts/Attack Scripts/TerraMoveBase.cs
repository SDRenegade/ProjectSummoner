using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerraMoveBase
{
    protected TerraAttack terraAttack;
    protected TerraMoveSO terraMoveSO;

    public TerraMoveBase(TerraAttack terraAttack, TerraMoveSO terraMoveSO)
    {
        this.terraAttack = terraAttack;
        this.terraMoveSO = terraMoveSO;
    }

    public abstract void PostAttackEffect(DirectAttackLog directAttackLog, BattleSystem battleSystem);

    public abstract void AddMoveListeners(BattleSystem battleSystem);

    public abstract void RemoveMoveListeners(BattleSystem battleSystem);

    public TerraAttack GetTerraAttack() { return terraAttack; }

    public TerraMoveSO GetTerraMoveSO() { return  terraMoveSO; }
}
