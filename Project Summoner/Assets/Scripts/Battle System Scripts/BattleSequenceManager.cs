using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceManager : MonoBehaviour
{
    private static BattleSequenceManager instance;

    [SerializeField] private BattleStage battleStage;
    [SerializeField] private LookAtPathFollower pathFollower;
    [SerializeField] private BattleCamera battleCam;
    [Header("Sequences")]
    [SerializeField] private ActionSequence introSequence;
    [SerializeField] private ActionSequence idleBattlefieldSequence;
    [SerializeField] private ActionSequence battleActionSequence;
    [Space]
    [SerializeField] private List<LookAtPath> introPathList;
    [Space]
    [SerializeField] private List<LookAtPath> idleBattlefieldPathList; // TODO Make a randomized path follower class

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void InitIntroSequence(Battlefield battlefield)
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
        for (int i = 0; i < battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            // Need a temp variable for i since the lambda expression will use the i value for
            // when the entire loop has finished since the action isn't being executed right away
            int iValue = i;
            introSequence.AddTaskByTime(() => {
                Transform terraTransform = battleStage.GetTerraObject(battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[iValue]).transform;
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
        for (int i = 0; i < battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            // Need a temp variable for i since the lambda expression will use the i value for
            // when the entire loop has finished since the action isn't being executed right away
            int iValue = i;
            introSequence.AddTaskByTime(() => {
                Transform terraTransform = battleStage.GetTerraObject(battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[iValue]).transform;
                Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
                battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, false);
            }, introDuration);
            introDuration += 1.5f;
        }

        introSequence.AddTaskByTime(() => StartIdleBattlefieldSequence(battlefield), introDuration);

        introSequence.SetDuration(introDuration);
    }

    private void InitIdleBattlefieldSequence(Battlefield battlefield)
    {
        float idleBattlefieldDuration = 60f;

        idleBattlefieldSequence.AddTaskByTime(() => {
            pathFollower.SetLookAtPath(idleBattlefieldPathList);
            pathFollower.SetSpeed(3.2f);
            pathFollower.SetIsLoop(true);
            pathFollower.SetIsActive(true);
        }, 0f);

        idleBattlefieldSequence.SetDuration(idleBattlefieldDuration);
    }

    public void AddBattleActionToSequence(IBattleSequence battleSequence)
    {
        AddBattleActionToSequence(battleSequence.GetBattleSequence(battleStage, battleCam));
    }

    public void AddBattleActionToSequence(Dictionary<Action, float> actionTasksByTime)
    {
        float sequenceDuration = 0f;


    }

    public void StartIntroSequence(Battlefield battlefield)
    {
        InitIntroSequence(battlefield);
        introSequence.StartSequence();
    }

    public void StartIdleBattlefieldSequence(Battlefield battlefield)
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if(battleActionSequence.IsPlaying())
            battleActionSequence.StopSequence();

        InitIdleBattlefieldSequence(battlefield);
        idleBattlefieldSequence.StartSequence();
    }

    public void StartBattleActionSequence(IBattleSequence battleSequence)
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if (idleBattlefieldSequence.IsPlaying())
            idleBattlefieldSequence.StopSequence();

        battleActionSequence.StartSequence();
    }

    public void AddTerraStatChangeBattleSequence()
    {
        // TODO 
    }

    // TODO Add methods for recoil, canceled attack, missed attack, attack charging

    public static BattleSequenceManager GetInstance() { return instance; }
}
