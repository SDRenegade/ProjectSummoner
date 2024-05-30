using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceManager : MonoBehaviour
{
    private static BattleSequenceManager instance;

    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private BattleStage battleStage;
    [SerializeField] private BattleDialogUI battleDialogUI;
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
        battleSystem.OnAttackDeclaration += AddAttackDeclarationToSequence;
        battleSystem.OnStartTerraAttack += AddAttackerAnimationToSequence;
        battleSystem.OnDirectAttackHit += AddAttackHitToSequence;
        battleSystem.OnSwitchTerra += AddTerraSwitchToSequence;
        battleSystem.OnEscapeAttempt += AddEscapeAttemptToSequence;
        battleSystem.OnCaptureAttempt += AddCaptureAttemptToSequence;

        introSequence.OnSequenceComplete += ShowStatusBars;
        introSequence.OnSequenceComplete += ExitInitBattleState;

        battleActionSequence.OnSequenceStart += HideActionSelectionHUD;
        battleActionSequence.OnSequenceComplete += ClearSequenceAndStartNextAction;
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

    public void StartIntroSequence(object sender, EventArgs eventArgs)
    {
        // Temp Removed Intro sequence for testing
        //InitIntroSequence(battleSystem.GetBattlefield());
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

    private void ShowStatusBars(object sender, EventArgs eventArgs)
    {
        battleSystem.GetBattleHUD().ShowTerraStatusBars();
    }

    private void ExitInitBattleState(object sender, EventArgs eventArgs)
    {
        battleSystem.ExitInitBattleState();
    }

    private void HideActionSelectionHUD(object sender, EventArgs eventArgs)
    {
        battleSystem.GetBattleHUD().CloseAllSelectionUI();
    }

    private void ClearSequenceAndStartNextAction(object sender, EventArgs eventArgs)
    {
        battleActionSequence.ClearSequence();
        battleDialogUI.HideDialog();
        battleSystem.NextCombatAction();
    }

    private void AddAttackDeclarationToSequence(object sender, TerraAttackEventArgs eventArgs)
    {
        TerraBattlePosition attackerPosition = eventArgs.GetTerraAttack().GetAttackerPosition();

        // Static shot at attacking terra
        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(attackerPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, attackerPosition.IsPrimarySide());

            battleDialogUI.SetDialog(BattleDialog.AttackUsedMsg(eventArgs.GetTerraAttack()));
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 1.25f);
    }

    private void AddAttackerAnimationToSequence(object sender, TerraAttackEventArgs eventArgs)
    {
        TerraBattlePosition attackerPosition = eventArgs.GetTerraAttack().GetAttackerPosition();

        // Terra attack animation
        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(attackerPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetAttackLookAt(terraOffsetPos, terraTransform.eulerAngles, attackerPosition.IsPrimarySide());
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 2f);
    }

    private void AddAttackHitToSequence(object sender, DirectAttackLogEventArgs eventArgs)
    {
        TerraBattlePosition targetPosition = eventArgs.GetDirectAttackLog().GetDefenderPosition();

        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(targetPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetAttackLookAt(terraOffsetPos, terraTransform.eulerAngles, targetPosition.IsPrimarySide());

            // TODO Set battle dialog if the attack was a crit or if the attack was not neutral
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 2f);
    }

    private void AddTerraSwitchToSequence(object sender, SwitchTerraEventArgs eventArgs)
    {
        TerraBattlePosition battlePosition = eventArgs.GetTerraSwitch().GetTerraBattlePosition();

        // Leading terra switch animation
        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(battlePosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, battlePosition.IsPrimarySide());
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 1.5f);

        // Switch terra gameobject and player throwing die animation
        Transform summonerTransform = battlePosition.IsPrimarySide() ? battleStage.GetPrimarySummonerGO().transform : battleStage.GetSecondarySummonerGO().transform;
        battleActionSequence.AddTaskByTime(() => {
            battleStage.SetTerraAtPosition(battlePosition);
            Vector3 terraOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, summonerTransform.eulerAngles, battlePosition.IsPrimarySide());
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 1.5f);

        // Switched-in terra switch animation
        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(battlePosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, battlePosition.IsPrimarySide());
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 1.5f);
    }

    private void AddEscapeAttemptToSequence(object sender, EscapeAttemptsEventArgs eventArgs)
    {
        bool isPrimarySide = eventArgs.GetEscapeAttempt().IsPrimarySide();

        // Static summoner shot
        Transform summonerTransform = eventArgs.GetEscapeAttempt().IsPrimarySide() ?
            battleStage.GetPrimarySummonerGO().transform : battleStage.GetSecondarySummonerGO().transform;
        battleActionSequence.AddTaskByTime(() => {
            Vector3 terraOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, summonerTransform.eulerAngles, isPrimarySide);
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 2f);
    }

    private void AddCaptureAttemptToSequence(object sender, CaptureAttemptEventArgs eventArgs)
    {
        bool isPrimarySide = eventArgs.GetCaptureAttempt().IsPrimarySide();
        TerraBattlePosition targetPosition = eventArgs.GetCaptureAttempt().GetTargetPosition();

        // Player throwing die animation
        Transform summonerTransform = isPrimarySide ? battleStage.GetPrimarySummonerGO().transform : battleStage.GetSecondarySummonerGO().transform;
        battleActionSequence.AddTaskByTime(() => {
            Vector3 terraOffsetPos = new Vector3(summonerTransform.position.x, summonerTransform.position.y + 1.75f, summonerTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, summonerTransform.eulerAngles, isPrimarySide);
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 2f);

        // Target terra capture animation
        battleActionSequence.AddTaskByTime(() => {
            Transform terraTransform = battleStage.GetTerraObject(targetPosition).transform;
            Vector3 terraOffsetPos = new Vector3(terraTransform.position.x, terraTransform.position.y + 1.75f, terraTransform.position.z);
            battleCam.SetStaticLookAt(terraOffsetPos, terraTransform.eulerAngles, targetPosition.IsPrimarySide());
        }, battleActionSequence.GetDuration());
        battleActionSequence.SetDuration(battleActionSequence.GetDuration() + 1.25f);
    }

    private void AddTerraFaintToSequence()
    {

    }


    public static BattleSequenceManager GetInstance() { return instance; }
}
