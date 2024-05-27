using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceManager : MonoBehaviour
{
    private static BattleSequenceManager instance;

    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private BattleStage battleStage;
    [SerializeField] private LookAtPathFollower pathFollower;
    [SerializeField] private BattleCamera battleCam;
    [Header("Sequences")]
    [SerializeField] private ActionSequence introSequence;
    [SerializeField] private ActionSequence idleBattlefieldSequence;
    [SerializeField] private ActionSequence battleActionSequence;
    [Header("Paths")]
    [SerializeField] private List<LookAtPath> introPathList;
    [SerializeField] private List<LookAtPath> idleBattlefieldPathList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        battleSystem.OnEndOfInitState += StartIntroSequence;
        battleSystem.OnEnteringActionSelectionState += StartIdleBattlefieldSequence;
        battleSystem.OnAttackDeclaration += AddActionToBattleSequence;

        introSequence.OnSequenceComplete += ExitInitBattleState;

        battleActionSequence.OnSequenceStart += HideActionSelectionHUD;
        battleActionSequence.OnSequenceComplete += ClearBattleSequenceOnCompletion;
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
        if (battleStage.GetSecondarySummonerGO() != null) {
            introSequence.AddTaskByTime(() => {
                Transform summonerTransform = battleStage.GetSecondarySummonerGO().transform;
                Vector3 summonerOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
                battleCam.SetStaticLookAt(summonerOffsetPos, summonerTransform.eulerAngles, false);
            }, introDuration);
            introDuration += 2f;
        }

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

        introSequence.SetDuration(introDuration);
    }

    private void InitIdleBattlefieldSequence()
    {
        idleBattlefieldSequence.SetIsLoop(true);

        idleBattlefieldSequence.OnSequenceStart -= SetIdleBattlefieldSequenceStartParams;
        idleBattlefieldSequence.OnSequenceStart += SetIdleBattlefieldSequenceStartParams;

        idleBattlefieldSequence.OnSequenceStop -= RemoveLookAtPathOnPathFollower;
        idleBattlefieldSequence.OnSequenceStop += RemoveLookAtPathOnPathFollower;
    }

    private void SetIdleBattlefieldSequenceStartParams(object sender, EventArgs eventArgs)
    {
        pathFollower.SetLookAtPath(idleBattlefieldPathList);
        pathFollower.SetSpeed(3.2f);
        pathFollower.SetIsLoop(true);
        pathFollower.SetIsActive(true);
    }

    private void RemoveLookAtPathOnPathFollower(object sender, EventArgs eventArgs)
    {
        pathFollower.SetLookAtPath(null);
    }

    public void AddActionToBattleSequence(object sender, BattleSequenceEventArgs eventArgs)
    {
        float sequenceDuration;
        foreach (KeyValuePair<Action, float> kvp in eventArgs.GetBattleSequence().GetTasksByTime(battleStage, battleCam, out sequenceDuration))
            battleActionSequence.AddTaskByTime(kvp.Key, kvp.Value + battleActionSequence.GetDuration());

        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + sequenceDuration);
    }

    public void StartIntroSequence(object sender, EventArgs eventArgs)
    {
        InitIntroSequence(battleSystem.GetBattlefield());
        introSequence.StartSequence();
    }

    public void StartIdleBattlefieldSequence(object sender, EventArgs eventArgs)
    {
        if (introSequence.IsPlaying())
            introSequence.StopSequence();
        if(battleActionSequence.IsPlaying())
            battleActionSequence.StopSequence();

        InitIdleBattlefieldSequence();
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

    private void ExitInitBattleState(object sender, EventArgs eventArgs)
    {
        battleSystem.ExitInitBattleState();
    }

    private void HideActionSelectionHUD(object sender, EventArgs eventArgs)
    {
        battleSystem.GetBattleHUD().CloseAllSelectionUI();
    }

    private void ClearBattleSequenceOnCompletion(object sender, EventArgs eventArgs)
    {
        battleActionSequence.ClearSequence();
        battleSystem.NextCombatAction();
    }

    public static BattleSequenceManager GetInstance() { return instance; }
}
