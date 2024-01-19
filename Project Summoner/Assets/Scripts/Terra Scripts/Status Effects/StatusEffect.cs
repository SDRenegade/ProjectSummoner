using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    protected Terra terra;

    public StatusEffect(Terra terra)
    {
        this.terra = terra;
    }

    public abstract void AddBattleEvent(BattleSystem battleSystem);

    public abstract void RemoveBattleEvent(BattleSystem battleSystem);

    //public abstract void AddWorldEvent();

    //public abstract void RemoveWorldEvent();
}
