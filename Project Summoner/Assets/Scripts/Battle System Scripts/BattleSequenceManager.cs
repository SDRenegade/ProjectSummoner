using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceManager : MonoBehaviour
{
    private static BattleSequenceManager instance;

    [SerializeField] private BattleStage battleStage;
    [Header("Sequences")]
    [SerializeField] private ActionSequence introSequence;
    [SerializeField] private ActionSequence idleBattlefieldSequence;
    [SerializeField] private ActionSequence battleActionSequence;
    [Header("Intro References")]
    [SerializeField] private BattleCamera battleCam;
    [SerializeField] private LookAtPathFollower introPath;
    [Header("Idle Battlefield References")]
    [SerializeField] private LookAtPathFollower idleBattlefieldPath; // TODO Make a randomized path follower class

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        introSequence.AddTaskByTime(() => introPath.SetIsActive(true), 0f);
        if (battleStage.GetSecondarySummonerGO() != null)
            introSequence.AddTaskByTime(() => battleCam.SetStaticLookAt(battleStage.GetSecondarySummonerGO().transform), 6f);
        introSequence.SetDuration(8f);
        introSequence.StartSequence();
    }

    public void StartIntroSequence()
    {
        introSequence.StartSequence();
    }

    public void StartIdleBattlefieldSequence()
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if(battleActionSequence.IsPlaying())
            battleActionSequence.StopSequence();

        idleBattlefieldSequence.StartSequence();
    }

    public void StartBattleActionSequence()
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if (idleBattlefieldSequence.IsPlaying())
            idleBattlefieldSequence.StopSequence();

        battleActionSequence.StartSequence();
    }

    public static BattleSequenceManager GetInstance() { return instance; }
}
