using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerDieItemStack
{
    [SerializeField] private SummonerDieBase summonerDieBase;
    [SerializeField] private int amount;

    public SummonerDieItemStack(SummonerDieBase summonerDieBase, int amount)
    {
        this.summonerDieBase = summonerDieBase;
        this.amount = amount;
    }

    public SummonerDieBase GetSummonerDieBase() { return summonerDieBase; }

    public void SetSummonerDieBase(SummonerDieBase summonerDieBase) { this.summonerDieBase = summonerDieBase; }

    public int GetAmount() { return amount; }

    public void SetAmount(int amount) {  this.amount = amount; }
}
