using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAttempt
{
    private TerraBattlePosition targetPosition;
    private SummonerDieBase summonerDie;
    private bool isPrimarySide;

    public CaptureAttempt(TerraBattlePosition targetPosition, SummonerDieBase summonerDie, bool isPrimarySide)
    {
        this.targetPosition = targetPosition;
        this.summonerDie = summonerDie;
        this.isPrimarySide = isPrimarySide;
    }

    public TerraBattlePosition GetTargetPosition() { return targetPosition; }

    public SummonerDieBase GetSummonerDie() { return summonerDie; }

    public bool IsPrimarySide() {  return isPrimarySide; }
}
