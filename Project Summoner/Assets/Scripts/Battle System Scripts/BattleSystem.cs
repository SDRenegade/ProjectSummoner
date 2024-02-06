using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public event EventHandler<BattleEventArgs> OnTerraSwitch; //Include BattleSide and the two Terra being switched
    public event EventHandler<BattleEventArgs> OnPlayerAttemptEscape; //Include Escape chance
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
    public event EventHandler<AttackChargingEventArgs> OnAttackCharging;
    public event EventHandler<AttackChargingEventArgs> OnAttackRecharging;
    public event EventHandler<TerraFaintedEventArgs> OnTerraFainted;
    public event EventHandler<DirectAttackLogEventArgs> OnPostAttack;
    public event EventHandler<BattleEventArgs> OnEndOfTurn;

    [SerializeField] private BattleHUD battleHUD;

    //Actor Positions
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform opponentTransform;
    [SerializeField] private Transform playerTerraTransform;
    [SerializeField] private Transform opponentTerraTransform;

    //Player & opposing summoner prefabs
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject opponentPrefab;
    
    //Player & opposing summoner gameobject's
    private GameObject playerBattleObject;
    //private GameObject opponentBattleObject
    //The currently battling Terras and their gameobject (Change the neme of the variables later)
    private TerraBattleObject playerLeadingTerra;
    private TerraBattleObject wildTerra;

    private bool isMatchFinished;
    private BattleType battleType;
    private BattleAI primarySideAI; // Might move BattleAI to the BattleSide classes
    private BattleAI secondarySideAI;
    private Battlefield battlefield;
    private BattleStateManager battleStateManager;
    private BattleActionManager battleActionManager;


    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isMatchFinished = false;
        battleType = BattleLoader.GetInstance().GetBattleType();

        SetupBattleScene();

        battlefield = new Battlefield(playerBattleObject.GetComponent<TerraParty>(), wildTerra.GetTerra());
        //Initialize the existing status conditions on the terra in the event system
        TerraBattlePosition primarySideTerraBattlePosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0];
        TerraBattlePosition secondarySideTerraBattlePosition = battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0];
        primarySideTerraBattlePosition.GetTerra().GetStatusEffect()?.AddBattleActions(primarySideTerraBattlePosition, this);
        secondarySideTerraBattlePosition.GetTerra().GetStatusEffect()?.AddBattleActions(secondarySideTerraBattlePosition, this);

        UpdateTerraStatusBars();

        primarySideAI = null;
        secondarySideAI = new WildTerraAI();

        //--- (Temp) Hard-coding the leading terra held item until new system is added ---
        //primarySideTerraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Cheri Berry").CreateItemBase());
        //secondarySideTerraBattlePosition.GetTerra().SetHeldItem(SODatabase.GetInstance().GetItemByName("Leftovers").CreateItemBase());

        primarySideTerraBattlePosition.GetTerra().GetHeldItem()?.AddBattleActions(primarySideTerraBattlePosition, this);
        secondarySideTerraBattlePosition.GetTerra().GetHeldItem()?.AddBattleActions(secondarySideTerraBattlePosition, this);

        //Logging the item that each leading terra is holding
        if (primarySideTerraBattlePosition.GetTerra().GetHeldItem() != null)
            Debug.Log("Primary Leading Terra Item: " + primarySideTerraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());
        if (secondarySideTerraBattlePosition.GetTerra().GetHeldItem() != null)
            Debug.Log("Secondary Leading Terra Item: " + secondarySideTerraBattlePosition.GetTerra().GetHeldItem().GetItemSO().GetItemName());

        //--- (Temp) Change second argument once more battle positions are added ---
        battleActionManager = new BattleActionManager(this, 2);
        battleStateManager = new BattleStateManager(this);
    }

    private void SetupBattleScene()
    {
        battleHUD.CloseAllSelectionUI();

        if (BattleLoader.GetInstance().GetPlayerTerraList() != null) {
            playerBattleObject = Instantiate(playerPrefab);
            playerBattleObject.transform.position = playerTransform.position;
            playerBattleObject.transform.eulerAngles = playerTransform.eulerAngles;
            TerraParty playerParty = playerBattleObject.GetComponent<TerraParty>();
            playerParty.AddPartyMemberList(BattleLoader.GetInstance().GetPlayerTerraList());

            playerLeadingTerra = new TerraBattleObject(playerParty.GetTerraList()[0]);
            playerLeadingTerra.SetTerraGO(Instantiate(playerLeadingTerra.GetTerra().GetTerraBase().GetTerraGameObject()));
            playerLeadingTerra.GetTerraGO().transform.position = playerTerraTransform.position;
            playerLeadingTerra.GetTerraGO().transform.eulerAngles = playerTerraTransform.eulerAngles;
        }
        else
            Debug.Log("No player terra party detected in BattleLoader");

        if (battleType == BattleType.WILD) {
            if (BattleLoader.GetInstance().GetWildTerra() != null) {
                wildTerra = new TerraBattleObject(BattleLoader.GetInstance().GetWildTerra());
                wildTerra.SetTerraGO(Instantiate(wildTerra.GetTerra().GetTerraBase().GetTerraGameObject()));
                wildTerra.GetTerraGO().transform.position = opponentTerraTransform.position;
                wildTerra.GetTerraGO().transform.eulerAngles = opponentTerraTransform.eulerAngles;
            }
            else
                Debug.Log("No opponenet object detected in BattleLoader");
        }

        BattleLoader.GetInstance().Clear();
    }

    public void UpdateTerraStatusBars()
    {
        battleHUD.UpdateTerraStatusBars(battlefield);
    }

    //TODO Find a way to pass the terrraBattlePosition of the action you are choosing
    public void OpenMoveSelectionUI()
    {
        TerraBattlePosition terraBattlePosition = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0];

        //*** Opening Move Selection UI Event ***
        OpeningMoveSelectionUIEventArgs openingMoveSelectionUIEventArgs = InvokeOnOpeningMoveSelectionUI(terraBattlePosition);

        if (openingMoveSelectionUIEventArgs.IsMoveSelectionCanceled()) {
            //Check if all battle positions are ready. If so, switch to combat state.
            if (battleActionManager.AddReadyBattlePosition())
                EndActionSelection();
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
                battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0],
                battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0],
                struggle);
            battleActionManager.GetTerraAttackList().Add(terraAttack);

            //Check if all battle positions are ready. If so, switch to combat state.
            if (battleActionManager.AddReadyBattlePosition())
                EndActionSelection();
        }
        else
            battleHUD.OpenMoveSelectionUI(moveList, openingMoveSelectionUIEventArgs.GetDisabledMoveIndicies());
    }

    //TODO Temp method being used for move button action until the terra battle position can be passed as
    //an argument. Have a method in the action manager that calls this method and then checks for all ready
    //positions, since this method is called in other classes such as Choice Band.
    public void MoveSelectionAction(int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        TerraMove selectedMove = battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0].GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null)
            return;
        if (selectedMove.GetCurrentPP() <= 0) {
            Debug.Log(BattleDialog.NoMovePowerPointsLeftMsg(selectedMove));
            return;
        }

        //Initializes the selected attack and add the new TerraAttack to the TerraAttackList
        TerraAttack terraAttack = new TerraAttack(
            battlefield.GetPrimaryBattleSide().GetTerraBattlePositionArr()[0],
            battlefield.GetSecondaryBattleSide().GetTerraBattlePositionArr()[0],
            selectedMove);
        battleActionManager.GetTerraAttackList().Add(terraAttack);
        //Add the selected moves battle actions into the event system
        terraAttack.GetTerraMoveBase()?.AddBattleActions(this);

        //Check if all battle positions are ready. If so, switch to combat state.
        if (battleActionManager.AddReadyBattlePosition())
            EndActionSelection();
    }

    //--- (Temp) This method is currently being called from Choice Band during the action slection event.
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
        terraAttack.GetTerraMoveBase()?.AddBattleActions(this);
    }

    public void EndActionSelection()
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void AttemptEscapeAction()
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;
        if(battleType != BattleType.WILD) {
            Debug.Log(BattleDialog.ATTEMPT_ESCAPE_SUMMONER_BATTLE);
            return;
        }

        //Escape Logic
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void UseSelectedItemAction(int itemIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        //Add item to the queued item list
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void SwitchTerraAction(int switchingTerraPartyIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        //Switching Terra Animation
        //Switch Logic
        UpdateTerraStatusBars();
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    //Method used when a terra is dealt damage that is not from a terra attack
    public int? DamageTerra(TerraBattlePosition terraBattlePosition, int? damage)
    {
        if (damage == null)
            return null;

        //*** Terra Damaged Event ***
        TerraDamagedEventArgs terraDamagedEventArgs = InvokeOnTerraDamaged(terraBattlePosition, damage);

        ApplyTerraDamage(terraBattlePosition, terraDamagedEventArgs.GetDamage());

        //*** Post Terra Damaged Event ***
        InvokeOnPostTerraDamaged(terraDamagedEventArgs);

        return terraDamagedEventArgs.GetDamage();
    }

    //Method called after damage calculate and terra damage events are invoked to actually apply the damage
    public void ApplyTerraDamage(TerraBattlePosition terraBattlePosition, int? damage)
    {
        if (damage == null)
            return;

        Debug.Log(BattleDialog.TerraDamagedMsg(terraBattlePosition.GetTerra(), (int)damage));
        terraBattlePosition.GetTerra().TakeDamage((int)damage);

        if (terraBattlePosition.GetTerra().GetCurrentHP() <= 0) {
            Debug.Log(BattleDialog.TerraFaintedMsg(terraBattlePosition.GetTerra()));
            //*** Terra Faint Event ***
            InvokeOnTerraFainted(terraBattlePosition);
            //--- (Temp) Match ends once a single terra faints, until parties are added ---
            isMatchFinished = true;
        }
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
        VolatileStatusEffectBase vStatusEffect = vStatusEffectSO.CreateVolatileStatusEffect(terraBattlePosition);

        //*** Volatile Status Effect Added Event ***
        VolatileStatusEffectAddedEventArgs vStatusEffectAddedEventArgs = InvokeOnVolatileStatusEffectAdded(terraBattlePosition, vStatusEffect);

        if (vStatusEffectAddedEventArgs.IsCanceled())
            return false;

        return terraBattlePosition.AddVolatileStatusEffect(vStatusEffectAddedEventArgs.GetVolatileStatusEffect(), this);
    }

    public bool CastRoll(float rollOdds)
    {
        return UnityEngine.Random.Range(0, 1f) < rollOdds;
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

    public BattleEventArgs InvokeOnTerraSwitch()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnTerraSwitch?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public BattleEventArgs InvokeOnPlayerAttemptEscape()
    {
        BattleEventArgs eventArgs = new BattleEventArgs(this);
        OnPlayerAttemptEscape?.Invoke(this, eventArgs);

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

    public bool IsMatchFinished() { return isMatchFinished; }

    public void SetMatchFinished(bool isMatchFinished) { this.isMatchFinished = isMatchFinished; }

    public BattleHUD GetBattleHUD() { return battleHUD; }

    public BattleType GetBattleType() { return battleType; }

    public BattleAI GetPrimarySideAI() { return primarySideAI; }

    public BattleAI GetSecondarySideAI() { return secondarySideAI; }

    public Battlefield GetBattlefield() { return battlefield; }

    public BattleActionManager GetBattleActionManager() { return battleActionManager; }
}