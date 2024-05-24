using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum BattleType
{
    Wild,
    Summoner
}

public class BattleSystem : MonoBehaviour
{
    public event EventHandler<BattleEventArgs> OnEnteringInitState;
    public event EventHandler<BattleEventArgs> OnStartOfTurn;
    public event EventHandler<EnteringActionSelectionEventArgs> OnEnteringActionSelection;
    public event EventHandler<OpeningMoveSelectionUIEventArgs> OnOpeningMoveSelectionUI;
    public event EventHandler<BattleEventArgs> OnActionSelection;
    public event EventHandler<BattleEventArgs> OnEnteringCombatState;
    public event EventHandler<EscapeAttemptsEventArgs> OnEscapeAttempt;
    public event EventHandler<CaptureAttemptEventArgs> OnCaptureAttempt;
    public event EventHandler<SwitchTerraEventArgs> OnSwitchTerra;
    public event EventHandler<AttackDeclarationEventArgs> OnAttackDeclaration;
    public event EventHandler<DirectAttackEventArgs> OnDirectAttack;
    public event EventHandler<DirectAttackLogEventArgs> OnAttackMissed;
    public event EventHandler<TerraDamageByTerraEventArgs> OnTerraDamageByTerra;
    public event EventHandler<TerraDamageByTerraEventArgs> OnPostTerraDamageByTerra;
    public event EventHandler<TerraDamagedEventArgs> OnTerraDamaged;
    public event EventHandler<TerraDamagedEventArgs> OnPostTerraDamaged;
    public event EventHandler<TerraHealedEventArgs> OnTerraHealed;
    public event EventHandler<StatChangeEventArgs> OnStatChange;
    public event EventHandler<StatusEffectAddedEventArgs> OnStatusEffectAdded;
    public event EventHandler<StatusEffectEventArgs> OnPostStatusEffectAdded;
    public event EventHandler<VolatileStatusEffectRollEventArgs> OnVolatileStatusEffectRoll;
    public event EventHandler<VolatileStatusEffectAddedEventArgs> OnVolatileStatusEffectAdded;
    public event EventHandler<VolatileStatusEffectEventArgs> OnPostVolatileStatusEffectAdded;
    public event EventHandler<AttackChargingEventArgs> OnAttackCharging;
    public event EventHandler<AttackChargingEventArgs> OnAttackRecharging;
    public event EventHandler<TerraFaintedEventArgs> OnTerraFainted;
    public event EventHandler<DirectAttackLogEventArgs> OnPostAttack;
    public event EventHandler<BattleEventArgs> OnEndOfTurn;

    [SerializeField] private BattleHUD battleHUD;
    [SerializeField] private BattleStage battleStage;
    
    private List<Terra> primaryTerraList;
    private List<Terra> secondaryTerraList;
    private List<SummonerDieItemStack> primarySummonerDieItemStackList;
    private List<SummonerDieItemStack> secondarySummonerDieItemStackList;

    private bool isBattleFinished;
    private BattleType battleType;
    private BattleFormat battleFormat;
    private BattleAI primarySideAI;
    private BattleAI secondarySideAI;
    private Battlefield battlefield;
    private BattleActionManager battleActionManager;
    private BattleStateManager battleStateManager;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        primaryTerraList = new List<Terra>();
        for(int i = 0; i < BattleLoader.GetInstance().GetPrimaryTerraList().Count; i++)
            primaryTerraList.Add(BattleLoader.GetInstance().GetPrimaryTerraList()[i]);

        secondaryTerraList = new List<Terra>();
        for (int i = 0; i < BattleLoader.GetInstance().GetSecondaryTerraList().Count; i++)
            secondaryTerraList.Add(BattleLoader.GetInstance().GetSecondaryTerraList()[i]);

        //--- Temp variables ---
        primarySummonerDieItemStackList = new List<SummonerDieItemStack> {
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Dragon Scale Die")), 1),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Worm Wood Die")), 2),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Summoner Die")), 3),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Dragon Scale Die")), 4),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Dragon Scale Die")), 5),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Worm Wood Die")), 6),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Worm Wood Die")), 7)
        };
        secondarySummonerDieItemStackList = new List<SummonerDieItemStack> {
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Dragon Scale Die")), 1),
            new SummonerDieItemStack(new SummonerDie(SODatabase.GetInstance().GetItemByName("Worm Wood Die")), 2)
        };

        isBattleFinished = false;
        battleType = BattleLoader.GetInstance().GetBattleType();
        battleFormat = BattleLoader.GetInstance().GetBattleFormat();
        primarySideAI = null;
        secondarySideAI = new WildTerraAI();
        battlefield = new Battlefield(battleFormat, primaryTerraList, secondaryTerraList);

        battleActionManager = new BattleActionManager(this);
        battleStateManager = new BattleStateManager(this);
    }

    public void UpdateTerraStatusBars()
    {
        battleHUD.UpdateTerraStatusBars(battlefield);
    }

    public void OpenMenuSelectionUI()
    {
        battleHUD.OpenMenuSelectionUI(battleActionManager);
    }

    public void ExitMenuSelectionUI()
    {
        battleHUD.ExitMenuSelection(battleActionManager);
    }

    public void ReturnToMenuSelection()
    {
        battleHUD.ReturnToMenuSelection(battlefield, battleActionManager);
    }

    public void OpenPartyMenuUI()
    {
        List<Terra> terraList = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide() ? primaryTerraList : secondaryTerraList;

        battleHUD.OpenPartyMenuUI(
            battleActionManager.GetCurrentTerraActionSelection(),
            terraList,
            false,
            (terraBattlePosition, terraSwitch) => {
                ReadyBattleAction(new SwitchBattleAction(terraBattlePosition, terraSwitch));
            },
            this);
    }

    public void OpenForceSwitchPartyMenuUI(TerraBattlePosition activeTerraPosition, bool isPrimarySide, Action<TerraBattlePosition, TerraSwitch> switchAction)
    {
        List<Terra> terraList = isPrimarySide ? primaryTerraList : secondaryTerraList;
        battleHUD.OpenPartyMenuUI(activeTerraPosition, terraList, true, switchAction, this);
    }

    public void OpenSummonerDieMenuUI()
    {
        List<SummonerDieItemStack> summonerDieList = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide() ? primarySummonerDieItemStackList : secondarySummonerDieItemStackList;
        battleHUD.OpenSummonerDieMenuUI(summonerDieList);
    }

    public void IterateSummonerDieSlider(int offset)
    {
        bool isPrimarySide = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide();
        List<SummonerDieItemStack> summonerDieList = isPrimarySide ? primarySummonerDieItemStackList : secondarySummonerDieItemStackList;
        battleHUD.GetSummonerDieMenuUI().OpenSummonerDieMenuUI(summonerDieList, offset);
    }

    public void SummonerDieSelection(int summonerDieIndex)
    {
        bool isPrimarySide = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide();
        List<SummonerDieItemStack> summonerDieList = isPrimarySide ? primarySummonerDieItemStackList : secondarySummonerDieItemStackList;
        if (summonerDieIndex >= summonerDieList.Count)
            return;

        CaptureAttempt captureAttempt = new CaptureAttempt(null, summonerDieList[summonerDieIndex].GetSummonerDieBase(), isPrimarySide);
        battleActionManager.SetPendingCaptureAttempt(captureAttempt);
        OpenTargetSelectionUI();
    }

    public void EscapeSelection()
    {
        if (battleType != BattleType.Wild) {
            Debug.Log(BattleDialog.CANNOT_ESCAPE_SUMMONER_BATTLE);
            return;
        }
        if(battleActionManager.GetEscapeAttempt() != null) {
            Debug.Log(BattleDialog.MULTIPLE_ESCAPE_ATTEMPTS);
            return;
        }

        EscapeAttempt escapeAttempt = new EscapeAttempt(battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide());
        ReadyBattleAction(new EscapeAttemptBattleAction(battleActionManager.GetCurrentTerraActionSelection(), escapeAttempt));
    }

    public void OpenMoveSelectionUI()
    {
        TerraBattlePosition terraBattlePosition = battleActionManager.GetCurrentTerraActionSelection();

        //*** Opening Move Selection UI Event ***
        OpeningMoveSelectionUIEventArgs openingMoveSelectionUIEventArgs = InvokeOnOpeningMoveSelectionUI(terraBattlePosition);

        if (openingMoveSelectionUIEventArgs.IsMoveSelectionCanceled()) {
            ReadyBattleAction(null);
            return;
        }

        //Create a list of available moves indicies after acounting for null move slots and disabled moves
        List<TerraMove> moveList = terraBattlePosition.GetTerra().GetMoves();
        List<int> availableMoveIndicies = new List<int>() { 0, 1, 2, 3 };
        for (int i = availableMoveIndicies.Count - 1; i >= 0; i--) {
            if (availableMoveIndicies[i] >= terraBattlePosition.GetTerra().GetMoves().Count || terraBattlePosition.GetTerra().GetMoves()[availableMoveIndicies[i]].GetCurrentPP() <= 0)
                availableMoveIndicies.RemoveAt(i);
        }
        foreach (int index in openingMoveSelectionUIEventArgs.GetDisabledMoveIndicies())
            availableMoveIndicies.Remove(index);

        if (availableMoveIndicies.Count == 0) {
            TerraMove struggle = new TerraMove(SODatabase.GetInstance().GetTerraMoveByName("Struggle"));
            //Initializes the selected attack and add the new TerraAttack to the TerraAttackList
            TerraAttack terraAttack = new TerraAttack(
                terraBattlePosition,
                battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0],
                struggle);
            ReadyBattleAction(new TerraAttackBattleAction(terraBattlePosition, terraAttack));
        }
        else
            battleHUD.OpenMoveSelectionUI(moveList, openingMoveSelectionUIEventArgs.GetDisabledMoveIndicies());
    }

    public void MoveSelectionAction(int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;
        TerraBattlePosition terraBattlePosition = battleActionManager.GetCurrentTerraActionSelection();
        TerraMove selectedMove = terraBattlePosition.GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null)
            return;
        if (selectedMove.GetCurrentPP() <= 0) {
            Debug.Log(BattleDialog.NoMovePowerPointsLeftMsg(selectedMove));
            return;
        }

        //Creating a list of all terra battle positions that are targetable
        List<TerraBattlePosition> targetablePositionList = new List<TerraBattlePosition>();
        TerraBattlePosition[] primaryTerraBattlePositions = GetBattlefield().GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] secondaryTerraBattlePositions = GetBattlefield().GetSecondaryBattleSide().GetTerraBattlePositionArr();
        for (int i = 0; i < primaryTerraBattlePositions.Length; i++) {
            if (primaryTerraBattlePositions[i] == terraBattlePosition || primaryTerraBattlePositions[i].GetTerra() == null)
                continue;
            targetablePositionList.Add(primaryTerraBattlePositions[i]);
        }
        for (int i = 0; i < secondaryTerraBattlePositions.Length; i++) {
            if (secondaryTerraBattlePositions[i].GetTerra() != null)
                targetablePositionList.Add(secondaryTerraBattlePositions[i]);
        }

        if (targetablePositionList.Count > 1 && selectedMove.GetMoveSO().IsTargetSelectable()) {
            //If we just put null for target the compiler can't tell which constructor to use. So, we
            //initialize a TerraBattlePosition to be null and pass that.
            TerraBattlePosition nullTarget = null;
            TerraAttack pendingTerraAttack = new TerraAttack(
                terraBattlePosition,
                nullTarget,
                selectedMove);
            battleActionManager.SetPendingTerraAttack(pendingTerraAttack);
            OpenTargetSelectionUI();
        }
        else if(selectedMove.GetMoveSO().IsSelfTargeting()) {
            //Initializes the a new terra attack with the selected move and defender position to be the
            //same as the attacker position
            TerraAttack terraAttack = new TerraAttack(
                terraBattlePosition,
                terraBattlePosition,
                selectedMove);
            ReadyBattleAction(new TerraAttackBattleAction(terraBattlePosition, terraAttack));
        }
        else {
            //Initializes the a new terra attack with the selected move and defender positions to be all
            //targetable positions
            TerraAttack terraAttack = new TerraAttack(
                terraBattlePosition,
                targetablePositionList,
                selectedMove);
            ReadyBattleAction(new TerraAttackBattleAction(terraBattlePosition, terraAttack));
        }
    }

    // If true, force adds a terra attack to the terra attack list instead of adding a battle action to
    // the stack. This prevents the player from undoing the action.
    public bool ForceTerraAttackSelection(TerraBattlePosition attackerPosition, List<TerraBattlePosition> defenderList, int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return false;
        TerraMove selectedMove = attackerPosition.GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null)
            return false;
        if (selectedMove.GetCurrentPP() <= 0) {
            Debug.Log(BattleDialog.NoMovePowerPointsLeftMsg(selectedMove));
            return false;
        }

        //Initializes the selected attack and add the new TerraAttack to the TerraAttackList
        TerraAttack terraAttack = new TerraAttack(attackerPosition, defenderList, selectedMove);
        battleActionManager.GetTerraAttackList().Add(terraAttack);
        //Add the selected moves battle actions into the event system
        terraAttack.GetTerraMoveBase()?.AddMoveListeners(this);

        return true;
    }

    public void OpenTargetSelectionUI()
    {
        if (battleActionManager.GetPendingTerraAttack() == null && battleActionManager.GetPendingCaptureAttempt() == null)
            return;

        TerraBattlePosition[] targetableAllyTerraPositions = new TerraBattlePosition[battleFormat.NumberOfLeadingPositions()];
        TerraBattlePosition[] targetableOpponentTerraPositions = new TerraBattlePosition[battleFormat.NumberOfLeadingPositions()];
        if (battleActionManager.GetPendingTerraAttack() != null) {
            TerraBattlePosition attackerPosition = battleActionManager.GetPendingTerraAttack().GetAttackerPosition();
            TerraBattlePosition[] allyTerraPositions = attackerPosition.IsPrimarySide() ? battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr() : battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();
            TerraBattlePosition[] opponentTerraPositions = attackerPosition.IsPrimarySide() ? battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr() : battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
            for (int i = 0; i < battleFormat.NumberOfLeadingPositions(); i++) {
                if (allyTerraPositions[i] != attackerPosition)
                    targetableAllyTerraPositions[i] = allyTerraPositions[i];
                targetableOpponentTerraPositions[i] = opponentTerraPositions[i];
            }
        }
        else {
            bool isPrimarySide = battleActionManager.GetPendingCaptureAttempt().IsPrimarySide();
            TerraBattlePosition[] opponentTerraPositions = isPrimarySide ? battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr() : battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
            for (int i = 0; i < opponentTerraPositions.Length; i++)
                targetableOpponentTerraPositions[i] = opponentTerraPositions[i];
        }

        UpdateTerraStatusBars();
        battleHUD.OpenTargetSelectionUI(targetableOpponentTerraPositions, targetableAllyTerraPositions);
    }

    public void TargetSelection(int positionIndex)
    {
        TerraBattlePosition targetTerraPosition = null;
        TerraBattlePosition[] opponentTerraPositions = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide() ? battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr() : battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        TerraBattlePosition[] allyTerraPositions = battleActionManager.GetCurrentTerraActionSelection().IsPrimarySide() ? battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr() : battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();
        if (positionIndex == 0)
            targetTerraPosition = opponentTerraPositions[0];
        else if(positionIndex == 1)
            targetTerraPosition = opponentTerraPositions[1];
        else if(positionIndex == 2)
            targetTerraPosition = allyTerraPositions[0];
        else if(positionIndex == 3)
            targetTerraPosition = allyTerraPositions[1];

        if (targetTerraPosition != null) {
            if(battleActionManager.GetPendingTerraAttack() != null) {
                battleActionManager.PushPendingTerraAttack(targetTerraPosition);
                AddReadyBattlePosition();
            }
            else if(battleActionManager.GetPendingCaptureAttempt() != null) {
                battleActionManager.PushPendingCaptureAttempt(targetTerraPosition);
                AddReadyBattlePosition();
            }
        }
        else
            Debug.LogError("The position index " + positionIndex + " is not a valid target.");
    }

    public void ExitTargetSelectionUI()
    {
        if (battleActionManager.GetPendingTerraAttack() != null) {
            battleActionManager.SetPendingTerraAttack(null);
            OpenMoveSelectionUI();
        }
        else if (battleActionManager.GetPendingCaptureAttempt() != null) {
            battleActionManager.SetPendingCaptureAttempt(null);
            OpenSummonerDieMenuUI();
        }
        else
            OpenMenuSelectionUI();
    }

    private void ReadyBattleAction(BattleAction battleAction)
    {
        if(battleAction != null)
            battleActionManager.AddBattleActionToStack(battleAction);

        AddReadyBattlePosition();
    }

    private void AddReadyBattlePosition()
    {
        battleActionManager.AddReadyBattlePosition();
        //Check if all battle positions are ready. If so, switch to combat state. Else, transition to
        //next terra action selection.
        if (battleActionManager.IsAllBattlePositionsReady())
            EndActionSelection();
        else if (battleActionManager.GetCurrentTerraActionSelection() != null)
            OpenMenuSelectionUI();
    }

    public void EndActionSelection()
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        battleActionManager.ProcessActionStacks(this);
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void EscapeAttempt(EscapeAttempt escapeAttempt)
    {
        if (escapeAttempt == null)
            return;

        //*** Escape Attempt Event ***
        EscapeAttemptsEventArgs escapeAttemptEventArgs = InvokeOnEscapeAttempt(escapeAttempt);

        if(escapeAttemptEventArgs.IsCanceled()) {
            Debug.Log(BattleDialog.ESCAPE_ATTEMPT_FAILED);
        }
        else if(escapeAttemptEventArgs.IsMustHit()) {
            Debug.Log(BattleDialog.ESCAPE_ATTEMPT_SUCCESS);
            isBattleFinished = true;
        }
        else {
            List<Terra> escapingTerraList;
            List<Terra> wildTerraList;
            if (escapeAttempt.IsPrimarySide()) {
                escapingTerraList = primaryTerraList;
                wildTerraList = secondaryTerraList;
            }
            else {
                escapingTerraList = secondaryTerraList;
                wildTerraList = primaryTerraList;
            }

            bool hasEscaped = CombatCalculator.EscapeAttemptCalculation(escapingTerraList, wildTerraList);
            if (hasEscaped) {
                Debug.Log(BattleDialog.ESCAPE_ATTEMPT_SUCCESS);
                isBattleFinished = true;
            }
            else
                Debug.Log(BattleDialog.ESCAPE_ATTEMPT_FAILED);
        }
    }

    public void CaptureAttempt(CaptureAttempt captureAttempt)
    {
        if (captureAttempt == null)
            return;

        //*** Capture Attempt Event ***
        CaptureAttemptEventArgs eventArgs = InvokeOnCaptureAttempt(captureAttempt);

        if (eventArgs.IsCanceled())
            return;

        if(CombatCalculator.CaptureAttemptCalculation(captureAttempt, this)) {
            Debug.Log(BattleDialog.CaptureAttemptSuccess(captureAttempt.GetTargetPosition().GetTerra()));
            List<Terra> playerPermanentTerraList = captureAttempt.IsPrimarySide() ? BattleLoader.GetInstance().GetPrimaryTerraList() : BattleLoader.GetInstance().GetSecondaryTerraList();
            List<Terra> opponentTerraList = captureAttempt.IsPrimarySide() ? secondaryTerraList : primaryTerraList;
            playerPermanentTerraList.Add(captureAttempt.GetTargetPosition().GetTerra());
            opponentTerraList.Remove(captureAttempt.GetTargetPosition().GetTerra());
            captureAttempt.GetTargetPosition().SetTerra(null);

            if (!TerraUtils.HasLivingPartyMember(opponentTerraList))
                EndBattle();
        }
        else
            Debug.Log(BattleDialog.CaptureAttemptFailed(captureAttempt.GetTargetPosition().GetTerra()));
    }

    public void SwitchTerra(TerraSwitch terraSwitch)
    {
        if (terraSwitch == null)
            return;

        //*** Switch Terra Event ***
        SwitchTerraEventArgs eventArgs = InvokeOnSwitchTerra(terraSwitch);

        if (eventArgs.IsCanceled())
            return;

        List<Terra> terraList = terraSwitch.GetTerraBattlePosition().IsPrimarySide() ? primaryTerraList : secondaryTerraList;
        Debug.Log(BattleDialog.SwitchTerraMsg(terraSwitch, terraList));
        Terra tmp = terraSwitch.GetTerraBattlePosition().GetTerra();
        terraSwitch.GetTerraBattlePosition().SetTerra(terraList[terraSwitch.GetBenchPositionIndex()]);
        terraList[terraSwitch.GetBenchPositionIndex()] = tmp;
        if(terraSwitch.IsPrimarySide())
            battlefield.GetPrimaryBattleSide().UpdateLeadingTerra(terraList);
        else
            battlefield.GetSecondaryBattleSide().UpdateLeadingTerra(terraList);
        battleStage.SetTerraAtPosition(terraSwitch.GetTerraBattlePosition());
        UpdateTerraStatusBars();
    }

    //Method used when a terra is dealt damage that is not from a terra attack
    public int? DamageTerra(TerraBattlePosition terraBattlePosition, int? damage)
    {
        if (terraBattlePosition.GetTerra() == null || damage == null)
            return null;

        //*** Terra Damaged Event ***
        TerraDamagedEventArgs terraDamagedEventArgs = InvokeOnTerraDamaged(terraBattlePosition, damage);

        ApplyDamage(terraBattlePosition, terraDamagedEventArgs.GetDamage());

        //*** Post Terra Damaged Event ***
        InvokeOnPostTerraDamaged(terraDamagedEventArgs);

        return terraDamagedEventArgs.GetDamage();
    }

    //Method called after damage calculate and terra damage events are invoked to actually apply the damage
    public void ApplyDamage(TerraBattlePosition terraBattlePosition, int? damage)
    {
        if (terraBattlePosition.GetTerra() == null || damage == null)
            return;

        Debug.Log(BattleDialog.TerraDamagedMsg(terraBattlePosition.GetTerra(), (int)damage));
        terraBattlePosition.GetTerra().TakeDamage((int)damage);

        if (terraBattlePosition.GetTerra().GetCurrentHP() <= 0)
            FaintTerra(terraBattlePosition);
    }

    private void FaintTerra(TerraBattlePosition terraBattlePosition)
    {
        Debug.Log(BattleDialog.TerraFaintedMsg(terraBattlePosition.GetTerra()));
        //*** Terra Faint Event ***
        InvokeOnTerraFainted(terraBattlePosition);

        bool isPrimarySide = terraBattlePosition.IsPrimarySide();
        List<Terra> terraList = isPrimarySide ? primaryTerraList : secondaryTerraList;
        int faintedTerraIndex = 0;
        for (int i = 0; i < battleFormat.NumberOfLeadingPositions(); i++) {
            if (i >= terraList.Count)
                break;
            if (terraBattlePosition.GetTerra() == terraList[i]) {
                faintedTerraIndex = i;
                break;
            }
        }

        terraBattlePosition.ResetBattlePosition(this);

        if (TerraUtils.HasLivingPartyMember(terraList))
            battleActionManager.GetFaintedTerraQueue().Enqueue(new FaintedTerra(terraBattlePosition, faintedTerraIndex, isPrimarySide));
        else
            EndBattle();
    }

    public void SwitchFaintedTerra()
    {
        if (battleActionManager.GetFaintedTerraQueue().Count == 0) {
            UpdateTerraStatusBars();
            battleActionManager.ResetActions(this);
            battleStateManager.SwitchState(battleStateManager.GetStartTurnState());
            return;
        }

        FaintedTerra faintedTerra = battleActionManager.GetFaintedTerraQueue().Dequeue();
        List<Terra> terraList = faintedTerra.IsPrimarySide() ? primaryTerraList : secondaryTerraList;
        if (TerraUtils.HasLivingBenchedPartyMember(terraList, battleFormat)) {
            BattleAI battleAI = faintedTerra.IsPrimarySide() ? primarySideAI : secondarySideAI;
            if (battleAI == null)
                OpenForceSwitchPartyMenuUI(
                    faintedTerra.GetTerraBattlePosition(),
                    faintedTerra.IsPrimarySide(),
                    (terraBattlePosition, terraSwitch) => {
                        SwitchTerra(terraSwitch);
                        SwitchFaintedTerra();
                    });
            else {
                int? switchIndex = battleAI.SwitchFaintedTerra(faintedTerra, this);
                if (switchIndex != null)
                    SwitchTerra(new TerraSwitch(
                        faintedTerra.GetTerraBattlePosition(),
                        (int)switchIndex,
                        faintedTerra.IsPrimarySide()));
                SwitchFaintedTerra();
            }
        }
        else {
            faintedTerra.GetTerraBattlePosition().SetTerra(null);
            battleStage.SetTerraAtPosition(faintedTerra.GetTerraBattlePosition());
            SwitchFaintedTerra();
        }
    }

    public int? HealTerra(TerraBattlePosition terraBattlePosition, int? healAmt)
    {
        if (terraBattlePosition.GetTerra() == null || healAmt == null)
            return null;

        //*** Terra Damaged Event ***
        TerraHealedEventArgs terraHealedEventArgs = InvokeOnTerraHealed(terraBattlePosition, healAmt);

        if (terraHealedEventArgs.GetHealAmt() != null) {
            Debug.Log(BattleDialog.TerraHealedMsg(terraBattlePosition.GetTerra(), (int)healAmt));
            terraBattlePosition.GetTerra().RecoverHP((int)terraHealedEventArgs.GetHealAmt());
        }

        return terraHealedEventArgs.GetHealAmt();
    }

    public void ChanageTerraStat(TerraBattlePosition terraBattlePosition, Stats stat, int modification)
    {
        if (terraBattlePosition.GetTerra() == null)
            return;

        //*** Stat Change Event ***
        StatChangeEventArgs statChangeEventArgs = InvokeOnStatChange(terraBattlePosition, stat, modification);

        if (!statChangeEventArgs.IsCanceled()) {
            terraBattlePosition.SetStatStage(stat, StatStagesExtension.ChangeStatStage(terraBattlePosition.GetStatStage(stat), statChangeEventArgs.GetModification()));
            Debug.Log(BattleDialog.StatStageChangeMsg(terraBattlePosition.GetTerra(), stat, terraBattlePosition.GetStatStage(stat), statChangeEventArgs.GetModification()));
        }
    }

    public bool AddStatusEffect(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO)
    {
        if (terraBattlePosition.GetTerra() == null || terraBattlePosition.GetTerra().HasStatusEffect())
            return false;

        //*** Status Effect Added Event ***
        StatusEffectAddedEventArgs statusEffectAddedEventArgs = InvokeOnStatusEffectAdded(terraBattlePosition, statusEffectSO);

        if (statusEffectAddedEventArgs.IsCanceled())
            return false;

        Debug.Log(BattleDialog.StatusInflictionMsg(terraBattlePosition.GetTerra(), statusEffectSO));
        terraBattlePosition.GetTerra().SetStatusEffect(statusEffectSO, terraBattlePosition, this);

        //*** Post Status Effect Added Event ***
        InvokeOnPostStatusEffectAdded(terraBattlePosition, statusEffectSO);

        return true;
    }

    public bool RollForVolatileStatusEffect(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, VolatileStatusEffectSO vStatusEffectSO, float rollOdds)
    {
        //*** Volatile Status Effect Roll Event ***
        VolatileStatusEffectRollEventArgs volatileStatusEffectRollEventArgs = InvokeOnVolatileStatusEffectRoll(attackerPosition, defenderPosition, vStatusEffectSO, rollOdds);

        return (volatileStatusEffectRollEventArgs.GetRollOdds() > UnityEngine.Random.Range(0, 1f)) ? AddVolatileStatusEffect(defenderPosition, vStatusEffectSO) : false;
    }

    public bool AddVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO)
    {
        if (terraBattlePosition.GetTerra() == null || terraBattlePosition.HasVolatileStatusEffect(vStatusEffectSO))
            return false;

        VolatileStatusEffectBase vStatusEffect = vStatusEffectSO.CreateVolatileStatusEffect(terraBattlePosition);

        //*** Volatile Status Effect Added Event ***
        VolatileStatusEffectAddedEventArgs vStatusEffectAddedEventArgs = InvokeOnVolatileStatusEffectAdded(terraBattlePosition, vStatusEffect);

        if (vStatusEffectAddedEventArgs.IsCanceled())
            return false;

        Debug.Log(BattleDialog.VolatileStatusInflictionMsg(terraBattlePosition.GetTerra(), vStatusEffectSO));
        terraBattlePosition.AddVolatileStatusEffect(vStatusEffectAddedEventArgs.GetVolatileStatusEffect(), this);

        //*** Post Volatile Status Effect Added Event ***
        InvokeOnPostVolatileStatusEffectAdded(terraBattlePosition, vStatusEffect);

        return true;
    }

    public void EndBattle()
    {
        isBattleFinished = true;
    }

    public BattleEventArgs InvokeOnEnteringInitState()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnEnteringInitState?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public BattleEventArgs InvokeOnStartOfTurn()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnStartOfTurn?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public EnteringActionSelectionEventArgs InvokeOnEnteringActionSelection(TerraBattlePosition terraBattlePosition)
    {
        EnteringActionSelectionEventArgs eventArgs = new EnteringActionSelectionEventArgs(terraBattlePosition, this);
        OnEnteringActionSelection?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public OpeningMoveSelectionUIEventArgs InvokeOnOpeningMoveSelectionUI(TerraBattlePosition terraBattlePosition)
    {
        OpeningMoveSelectionUIEventArgs eventArgs = new OpeningMoveSelectionUIEventArgs(terraBattlePosition, this);
        OnOpeningMoveSelectionUI?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public BattleEventArgs InvokeOnActionSelection()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnActionSelection?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public BattleEventArgs InvokeOnEnteringCombatState()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnEnteringCombatState?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public EscapeAttemptsEventArgs InvokeOnEscapeAttempt(EscapeAttempt escapeAttempt)
    {
        EscapeAttemptsEventArgs eventArgs = new EscapeAttemptsEventArgs(escapeAttempt, this);
        OnEscapeAttempt?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public CaptureAttemptEventArgs InvokeOnCaptureAttempt(CaptureAttempt captureAttempt)
    {
        CaptureAttemptEventArgs eventArgs = new CaptureAttemptEventArgs(captureAttempt, this);
        OnCaptureAttempt?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public SwitchTerraEventArgs InvokeOnSwitchTerra(TerraSwitch terraSwitch)
    {
        SwitchTerraEventArgs eventArgs = new SwitchTerraEventArgs(terraSwitch, this);
        OnSwitchTerra?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public AttackDeclarationEventArgs InvokeOnAttackDeclaration(TerraAttack terraAttack)
    {
        AttackDeclarationEventArgs eventArgs = new AttackDeclarationEventArgs(terraAttack, this);
        OnAttackDeclaration?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public DirectAttackEventArgs InvokeOnDirectAttack(DirectAttackParams directAttackParams)
    {
        DirectAttackEventArgs eventArgs = new DirectAttackEventArgs(directAttackParams, this);
        OnDirectAttack?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public DirectAttackLogEventArgs InvokeOnAttackMissed(DirectAttackLog directAttackLog)
    {
        DirectAttackLogEventArgs eventArgs = new DirectAttackLogEventArgs(directAttackLog, this);
        OnAttackMissed?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraDamageByTerraEventArgs InvokeOnTerraDamageByTerra(TerraAttack terraAttack, DirectAttackLog terraAttackLog)
    {
        TerraDamageByTerraEventArgs eventArgs = new TerraDamageByTerraEventArgs(terraAttack, terraAttackLog, this);
        OnTerraDamageByTerra?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraDamageByTerraEventArgs InvokeOnPostTerraDamageByTerra(TerraDamageByTerraEventArgs eventArgs)
    {
        OnPostTerraDamageByTerra?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraDamagedEventArgs InvokeOnTerraDamaged(TerraBattlePosition terraBattlePosition, int? damage)
    {
        TerraDamagedEventArgs eventArgs = new TerraDamagedEventArgs(terraBattlePosition, damage, this);
        OnTerraDamaged?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraDamagedEventArgs InvokeOnPostTerraDamaged(TerraDamagedEventArgs eventArgs)
    {
        OnPostTerraDamaged?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraHealedEventArgs InvokeOnTerraHealed(TerraBattlePosition terraBattlePosition, int? damage)
    {
        TerraHealedEventArgs eventArgs = new TerraHealedEventArgs(terraBattlePosition, damage, this);
        OnTerraHealed?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public StatChangeEventArgs InvokeOnStatChange(TerraBattlePosition terraBattlePosition, Stats stat, int modification)
    {
        StatChangeEventArgs eventArgs = new StatChangeEventArgs(terraBattlePosition, stat, modification, this);
        OnStatChange?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public StatusEffectAddedEventArgs InvokeOnStatusEffectAdded(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO)
    {
        StatusEffectAddedEventArgs eventArgs = new StatusEffectAddedEventArgs(terraBattlePosition, statusEffectSO, this);
        OnStatusEffectAdded?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public StatusEffectEventArgs InvokeOnPostStatusEffectAdded(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO)
    {
        StatusEffectEventArgs eventArgs = new StatusEffectEventArgs(terraBattlePosition, statusEffectSO, this);
        OnPostStatusEffectAdded?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public VolatileStatusEffectRollEventArgs InvokeOnVolatileStatusEffectRoll(TerraBattlePosition attackerPosition, TerraBattlePosition defenderPosition, VolatileStatusEffectSO vStatusEffectSO, float rollOdds)
    {
        VolatileStatusEffectRollEventArgs eventArgs = new VolatileStatusEffectRollEventArgs(attackerPosition, defenderPosition, vStatusEffectSO, rollOdds, this);
        OnVolatileStatusEffectRoll?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public VolatileStatusEffectAddedEventArgs InvokeOnVolatileStatusEffectAdded(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        VolatileStatusEffectAddedEventArgs eventArgs = new VolatileStatusEffectAddedEventArgs(terraBattlePosition, vStatusEffect, this);
        OnVolatileStatusEffectAdded?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public VolatileStatusEffectEventArgs InvokeOnPostVolatileStatusEffectAdded(TerraBattlePosition terraBattlePosition, VolatileStatusEffectBase vStatusEffect)
    {
        VolatileStatusEffectEventArgs eventArgs = new VolatileStatusEffectEventArgs(terraBattlePosition, vStatusEffect, this);
        OnPostVolatileStatusEffectAdded?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public AttackChargingEventArgs InvokeOnAttackCharging(TerraAttack terraAttack)
    {
        AttackChargingEventArgs eventArgs = new AttackChargingEventArgs(terraAttack, this);
        OnAttackCharging?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public AttackChargingEventArgs InvokeOnAttackRecharging(TerraAttack terraAttack)
    {
        AttackChargingEventArgs eventArgs = new AttackChargingEventArgs(terraAttack, this);
        OnAttackRecharging?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public TerraFaintedEventArgs InvokeOnTerraFainted(TerraBattlePosition terraBattlePosition)
    {
        TerraFaintedEventArgs eventArgs = new TerraFaintedEventArgs(terraBattlePosition, this);
        OnTerraFainted?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public DirectAttackLogEventArgs InvokeOnPostAttack(DirectAttackLog directAttackLog)
    {
        DirectAttackLogEventArgs eventArgs = new DirectAttackLogEventArgs(directAttackLog, this);
        OnPostAttack?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public BattleEventArgs InvokeOnEndOfTurn()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnEndOfTurn?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public bool IsBattleFinished() { return isBattleFinished; }

    public BattleHUD GetBattleHUD() { return battleHUD; }

    public BattleStage GetBattleStage() { return battleStage; }

    public List<Terra> GetPrimaryTerraList() { return primaryTerraList; }

    public List<Terra> GetSecondaryTerraList() { return secondaryTerraList; }

    public BattleType GetBattleType() { return battleType; }

    public BattleFormat GetBattleFormat() { return battleFormat; }

    public BattleAI GetPrimarySideAI() { return primarySideAI; }

    public BattleAI GetSecondarySideAI() { return secondarySideAI; }

    public Battlefield GetBattlefield() { return battlefield; }

    public BattleActionManager GetBattleActionManager() { return battleActionManager; }
}