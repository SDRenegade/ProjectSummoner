using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleType
{
    WILD,
    SUMMONER
}

public class BattleSystem : MonoBehaviour
{
    public event EventHandler<BattleEventArgs> OnStartOfTurn;
    public event EventHandler<EnteringActionSelectionEventArgs> OnEnteringActionSelection;
    public event EventHandler<OpeningMoveSelectionUIEventArgs> OnOpeningMoveSelectionUI;
    public event EventHandler<BattleEventArgs> OnActionSelection;
    public event EventHandler<BattleEventArgs> OnEnteringCombatState;
    public event EventHandler<BattleEventArgs> OnPlayerAttemptEscape; //Include Escape chance
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

    private bool isBattleFinished;
    private BattleType battleType;
    private BattleFormat battleFormat;
    private BattleAI primarySideAI;
    private BattleAI secondarySideAI;
    private Battlefield battlefield;
    private BattleStateManager battleStateManager;
    private BattleActionManager battleActionManager;


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

        isBattleFinished = false;
        battleType = BattleLoader.GetInstance().GetBattleType();
        battleFormat = BattleLoader.GetInstance().GetBattleFormat();
        primarySideAI = null;
        secondarySideAI = new WildTerraAI(secondaryTerraList);
        battlefield = new Battlefield(battleFormat, primaryTerraList, secondaryTerraList);

        InitBattleStage();
        InitBattleActions();
        UpdateTerraStatusBars(); //Might not be needed. Could be getting called elsewhere

        battleActionManager = new BattleActionManager(this);
        battleStateManager = new BattleStateManager(this);
    }

    private void InitBattleStage()
    {
        TerraBattlePosition[] primarySummonerTerraPositionList = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr();
        for (int i = 0; i < primarySummonerTerraPositionList.Length && i < primaryTerraList.Count; i++) {
            if (primaryTerraList[i] != null)
                battleStage.SetTerraAtPosition(primaryTerraList[i], true, i);
        }

        TerraBattlePosition[] secondarySummonerTerraPositionList = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr();
        for (int i = 0; i < secondarySummonerTerraPositionList.Length && i < secondaryTerraList.Count; i++) {
            if (secondaryTerraList[i] != null)
                battleStage.SetTerraAtPosition(secondaryTerraList[i], false, i);
        }
    }

    private void InitBattleActions()
    {
        //Initialize the existing status conditions and items on the terra in the event system
        for(int i = 0; i < battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            TerraBattlePosition terraBattlePosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[i];
            if (terraBattlePosition.GetTerra() == null)
                continue;

            terraBattlePosition.GetTerra().GetStatusEffect()?.AddStatusListeners(terraBattlePosition, this);
            //--- (Temp) Hard-coding the leading terra held item until new system is added ---
            terraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Persim Berry").CreateItemBase());
            terraBattlePosition.GetTerra().GetHeldItem()?.AddItemListeners(terraBattlePosition, this);

            //Logging the item that each leading terra is holding
            if (terraBattlePosition.GetTerra().GetHeldItem() != null)
                Debug.Log(terraBattlePosition.GetTerra() + " is holding the item: " + terraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());
        }

        for (int i = 0; i < battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr().Length; i++) {
            TerraBattlePosition terraBattlePosition = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[i];
            if (terraBattlePosition.GetTerra() == null)
                continue;

            terraBattlePosition.GetTerra().GetStatusEffect()?.AddStatusListeners(terraBattlePosition, this);
            //--- (Temp) Hard-coding the leading terra held item until new system is added ---
            //terraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Leftovers").CreateItemBase());
            terraBattlePosition.GetTerra().GetHeldItem()?.AddItemListeners(terraBattlePosition, this);

            //Logging the item that each leading terra is holding
            if (terraBattlePosition.GetTerra().GetHeldItem() != null)
                Debug.Log(terraBattlePosition.GetTerra() + " is holding the item: " + terraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());
        }
    }

    public void UpdateTerraStatusBars()
    {
        battleHUD.UpdateTerraStatusBars(battlefield, battleFormat);
    }

    public void OpenMenuSelectionUI()
    {
        battleHUD.OpenMenuSelectionUI(battleActionManager);
    }

    public void ExitMenuSelectionUI()
    {
        battleHUD.ExitMenuSelection(battleActionManager);
    }

    //TODO Specify which side is opening the party menu
    public void OpenPartyMenuUI()
    {
        battleHUD.OpenPartyMenuUI(
            battleActionManager.GetCurrentTerraActionSelection(),
            primaryTerraList,
            false,
            (terraBattlePosition, terraSwitch) => {
                SwitchTerraSelection(new SwitchBattleAction(terraBattlePosition, terraSwitch));
            },
            this);
    }

    public void OpenForceSwitchPartyMenuUI(TerraBattlePosition activeTerraPosition, bool isPrimarySide, Action<TerraBattlePosition, TerraSwitch> switchAction)
    {
        List<Terra> terraList = isPrimarySide ? primaryTerraList : secondaryTerraList;
        battleHUD.OpenPartyMenuUI(activeTerraPosition, terraList, true, switchAction, this);
    }

    public void ExitPartyMenuUI()
    {
        battleHUD.ExitPartyMenuUI(battlefield, battleFormat, battleActionManager);
    }

    public void OpenMoveSelectionUI()
    {
        TerraBattlePosition terraBattlePosition = battleActionManager.GetCurrentTerraActionSelection();

        //*** Opening Move Selection UI Event ***
        OpeningMoveSelectionUIEventArgs openingMoveSelectionUIEventArgs = InvokeOnOpeningMoveSelectionUI(terraBattlePosition);

        if (openingMoveSelectionUIEventArgs.IsMoveSelectionCanceled()) {
            AddReadyBattlePosition();
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
            battleActionManager.AddBattleActionToStack(new TerraAttackBattleAction(terraBattlePosition, terraAttack));

            AddReadyBattlePosition();
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
            OpenTargetSelectionUI(terraBattlePosition, battlefield);
        }
        else if(selectedMove.GetMoveSO().IsSelfTargeting()) {
            //Initializes the a new terra attack with the selected move and defender position to be the
            //same as the attacker position
            TerraAttack terraAttack = new TerraAttack(
                terraBattlePosition,
                terraBattlePosition,
                selectedMove);
            battleActionManager.AddBattleActionToStack(new TerraAttackBattleAction(terraBattlePosition, terraAttack));

            AddReadyBattlePosition();
        }
        else {
            //Initializes the a new terra attack with the selected move and defender positions to be all
            //targetable positions
            TerraAttack terraAttack = new TerraAttack(
                terraBattlePosition,
                targetablePositionList,
                selectedMove);
            battleActionManager.AddBattleActionToStack(new TerraAttackBattleAction(terraBattlePosition, terraAttack));

            AddReadyBattlePosition();
        }
    }

    //TODO Refactor this to work like the other MoveSelectionAction method
    //--- (Temp) This method is currently being called from Choice Band during the action selection event.
    //So, the ready position and the check for end of action selection method calls were removed.
    public void MoveSelectionAction(TerraBattlePosition attackerPosition, List<TerraBattlePosition> defenderList, int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        TerraMove selectedMove = attackerPosition.GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null)
            return;
        if (selectedMove.GetCurrentPP() <= 0) {
            Debug.Log(BattleDialog.NoMovePowerPointsLeftMsg(selectedMove));
            return;
        }

        //Initializes the selected attack and add the new TerraAttack to the TerraAttackList
        TerraAttack terraAttack = new TerraAttack(attackerPosition, defenderList, selectedMove);
        battleActionManager.GetTerraAttackList().Add(terraAttack);
        //Add the selected moves battle actions into the event system
        terraAttack.GetTerraMoveBase()?.AddMoveListeners(this);
    }

    public void OpenTargetSelectionUI(TerraBattlePosition terraBattlePosition, Battlefield battlefield)
    {
        battleHUD.OpenTargetSelectionUI(terraBattlePosition, battlefield);
    }

    public void TargetSelection(int positionIndex)
    {
        TerraBattlePosition targetTerraPosition = null;
        if(positionIndex == 0) {
            if (battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0] != battleActionManager.GetPendingTerraAttack().GetAttackerPosition())
                targetTerraPosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0];
            else
                targetTerraPosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[1];
        }
        else if(positionIndex == 1) {
            if (battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0].GetTerra() != null)
                targetTerraPosition = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0];
        }
        else if(positionIndex == 2) {
            if (battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[1].GetTerra() != null)
                targetTerraPosition = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[1];
        }

        if (targetTerraPosition != null) {
            battleActionManager.PushPendingTerraAttack(targetTerraPosition);
            AddReadyBattlePosition();
        }
        else
            Debug.LogError("The position index " + positionIndex + " is not a valid target.");
    }

    public void SwitchTerraSelection(SwitchBattleAction switchBattleAction)
    {
        battleActionManager.AddBattleActionToStack(switchBattleAction);
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

    public void EscapeAttempt()
    {
        if(battleType != BattleType.WILD) {
            Debug.Log(BattleDialog.ATTEMPT_ESCAPE_SUMMONER_BATTLE);
            return;
        }

        //Escape Logic
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void CatchTerraAttempt(int itemIndex)
    {
        //TODO Implement item active
    }

    public void SwitchTerra(TerraSwitch terraSwitch)
    {
        //*** Switch Terra Event ***
        SwitchTerraEventArgs eventArgs = InvokeOnSwitchTerra(terraSwitch);

        if (eventArgs.IsCanceled())
            return;

        List<Terra> terraList = terraSwitch.IsPrimarySide() ? primaryTerraList : secondaryTerraList;
        Debug.Log(BattleDialog.SwitchTerraMsg(terraSwitch, terraList));
        Terra tmp = primaryTerraList[terraSwitch.GetLeadingPositionIndex()];
        terraList[terraSwitch.GetLeadingPositionIndex()] = terraList[terraSwitch.GetBenchPositionIndex()];
        terraList[terraSwitch.GetBenchPositionIndex()] = tmp;
        if(terraSwitch.IsPrimarySide())
            battlefield.GetPrimaryBattleSide().UpdateLeadingTerra(terraList);
        else
            battlefield.GetSecondaryBattleSide().UpdateLeadingTerra(terraList);
        battleStage.SetTerraAtPosition(terraList[terraSwitch.GetLeadingPositionIndex()], terraSwitch.IsPrimarySide(), terraSwitch.GetLeadingPositionIndex());
        UpdateTerraStatusBars();
    }

    //Method used when a terra is dealt damage that is not from a terra attack
    public int? DamageTerra(TerraBattlePosition terraBattlePosition, int? damage)
    {
        if (damage == null)
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
        if (damage == null)
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

        bool isPrimarySide = terraBattlePosition.GetBattleSide().IsPrimarySide();
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

        if (HasLivingTerra(isPrimarySide))
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
        if(HasLivingBenchTerra(faintedTerra.IsPrimarySide())) {
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
                battleAI.SwitchFaintedTerra(faintedTerra);
                SwitchFaintedTerra();
            }
        }
        else
            SwitchFaintedTerra();
    }

    //TODO Move to a utils class
    private bool HasLivingTerra(bool isPrimarySide)
    {
        bool hasLivingTerra = false;
        List<Terra> terraList = isPrimarySide ? primaryTerraList : secondaryTerraList;
        for (int i = 0; i < terraList.Count; i++) {
            if (terraList[i].GetCurrentHP() > 0) {
                hasLivingTerra = true;
                break;
            }
        }

        return hasLivingTerra;
    }

    //TODO Move to a utils class
    private bool HasLivingBenchTerra(bool isPrimarySide)
    {
        bool hasLivingBenchTerra = false;
        List<Terra> terraList = isPrimarySide ? primaryTerraList : secondaryTerraList;
        int leadingTerraPositions = battleFormat.NumberOfLeadingPositions();
        for (int i = leadingTerraPositions; i < terraList.Count; i++) {
            if (terraList[i].GetCurrentHP() > 0) {
                hasLivingBenchTerra = true;
                break;
            }
        }

        return hasLivingBenchTerra;
    }

    public int? HealTerra(TerraBattlePosition terraBattlePosition, int? healAmt)
    {
        if (healAmt == null)
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
        //*** Stat Change Event ***
        StatChangeEventArgs statChangeEventArgs = InvokeOnStatChange(terraBattlePosition, stat, modification);

        if (!statChangeEventArgs.IsCanceled()) {
            terraBattlePosition.SetStatStage(stat, StatStagesExtension.ChangeStatStage(terraBattlePosition.GetStatStage(stat), statChangeEventArgs.GetModification()));
            Debug.Log(BattleDialog.StatStageChangeMsg(terraBattlePosition.GetTerra(), stat, terraBattlePosition.GetStatStage(stat), statChangeEventArgs.GetModification()));
        }
    }

    public bool AddStatusEffect(TerraBattlePosition terraBattlePosition, StatusEffectSO statusEffectSO)
    {
        if (terraBattlePosition.GetTerra().HasStatusEffect())
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
        
        return CastRoll(volatileStatusEffectRollEventArgs.GetRollOdds()) ? AddVolatileStatusEffect(defenderPosition, vStatusEffectSO) : false;
    }

    public bool AddVolatileStatusEffect(TerraBattlePosition terraBattlePosition, VolatileStatusEffectSO vStatusEffectSO)
    {
        if (terraBattlePosition.HasVolatileStatusEffect(vStatusEffectSO))
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

    //TODO Move to some utility class
    public bool CastRoll(float rollOdds)
    {
        return UnityEngine.Random.Range(0, 1f) < rollOdds;
    }

    public void EndBattle()
    {
        isBattleFinished = true;
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

    public BattleEventArgs InvokeOnPlayerAttemptEscape()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnPlayerAttemptEscape?.Invoke(this, eventArgs);

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

    public void SetBattleFinished(bool isBattleFinished) { this.isBattleFinished = isBattleFinished; }

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