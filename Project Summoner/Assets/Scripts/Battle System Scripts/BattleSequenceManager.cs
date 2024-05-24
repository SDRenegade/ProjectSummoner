using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceManager : MonoBehaviour
{
    private static BattleSequenceManager instance;

    [SerializeField] private BattleStage battleStage;
    [SerializeField] private LookAtPathFollower pathFollower;
    [Header("Sequences")]
    [SerializeField] private ActionSequence introSequence;
    [SerializeField] private ActionSequence idleBattlefieldSequence;
    [SerializeField] private ActionSequence battleActionSequence;
    [Header("Intro Path")]
    [SerializeField] private BattleCamera battleCam;
    [SerializeField] private List<LookAtPath> introPathList;
    [Header("Idle Battlefield Path")]
    [SerializeField] private List<LookAtPath> idleBattlefieldPathList; // TODO Make a randomized path follower class

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeIntroSequence();
    }

    private void InitializeIntroSequence()
    {
        float introDuration = 0f;

        introSequence.AddTaskByTime(() => {
            pathFollower.SetLookAtPath(introPathList);
            pathFollower.SetSpeed(32f);
            pathFollower.SetIsActive(true);
        }, 0f);
        introDuration = 6.5f;
        // Opponent intro
        if (battleStage.GetSecondarySummonerGO() != null)
            introSequence.AddTaskByTime(() => {
                Transform summonerTransform = battleStage.GetSecondarySummonerGO().transform;
                Vector3 summonerOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
                battleCam.SetStaticLookAt(summonerOffsetPos, summonerTransform.eulerAngles, false);
            }, introDuration);
        introDuration += 2f;
        // Player casting die animation
        introSequence.AddTaskByTime(() => {
            Transform summonerTransform = battleStage.GetPrimarySummonerGO().transform;
            Vector3 summonerOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
            battleCam.SetStaticLookAt(summonerOffsetPos, summonerTransform.eulerAngles, true);
        }, introDuration);
        introDuration += 1.5f;
        // Player terra summoning animation
        for (int i = 0; i < battleStage.GetPrimaryTerraGOArr().Length; i++) {
            // Need a temp variable for i since the lambda expression will use the i value for
            // when the entire loop has finished since the action isn't being executed right away
            int iValue = i;
            introSequence.AddTaskByTime(() => {
                Transform terraTransform = battleStage.GetPrimaryTerraGOArr()[iValue].transform;
                Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);

                battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, true);
            }, introDuration);
            introDuration += 1.5f;
        }
        // Opponent casting die animation
        introSequence.AddTaskByTime(() => {
            Transform summonerTransform = battleStage.GetSecondarySummonerGO().transform;
            Vector3 summonerOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
            battleCam.SetStaticLookAt(summonerOffsetPos, summonerTransform.eulerAngles, false);
        }, introDuration);
        introDuration += 1.5f;
        // Opponent terra summoning animation
        for (int i = 0; i < battleStage.GetSecondaryTerraGOArr().Length; i++) {
            // Need a temp variable for i since the lambda expression will use the i value for
            // when the entire loop has finished since the action isn't being executed right away
            int iValue = i;
            introSequence.AddTaskByTime(() => {
                Transform terraTransform = battleStage.GetSecondaryTerraGOArr()[iValue].transform;
                Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);

                battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, false);
            }, introDuration);
            introDuration += 1.5f;
        }
        introSequence.AddTaskByTime(() => StartIdleBattlefieldSequence(), introDuration);

        introSequence.SetDuration(introDuration);
        introSequence.StartSequence(); // Temp remove once StartIntroScenece is being called form this class
    }

    private void InitializeIdleBattlefieldSequence()
    {
        float idleBattlefieldDuration = 60f;

        idleBattlefieldSequence.AddTaskByTime(() => {
            pathFollower.SetLookAtPath(idleBattlefieldPathList);
            pathFollower.SetSpeed(3.5f);
            pathFollower.SetIsLoop(true);
            pathFollower.SetIsActive(true);
        }, 0f);

        idleBattlefieldSequence.SetDuration(idleBattlefieldDuration);
    }

    public void StartIntroSequence()
    {
        InitializeIntroSequence();
        introSequence.StartSequence();
    }

    public void StartIdleBattlefieldSequence()
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if(battleActionSequence.IsPlaying())
            battleActionSequence.StopSequence();

        InitializeIdleBattlefieldSequence();
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
