using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleType
{
    WILD,
    NPC_SUMMONER,
    PLAYER_SUMMONER
}

public class BattleSystem : MonoBehaviour
{
    //Battle Events
    public event EventHandler<BattleEventArgs> OnEnteringStartTurnState;
    public event EventHandler<BattleEventArgs> OnEnteringActionSelectionState;
    public event EventHandler<BattleEventArgs> OnActionSelection;
    public event EventHandler<BattleEventArgs> OnEnteringCombatState;
    public event EventHandler<BattleEventArgs> OnTerraSwitch; //Include BattleSide and the two Terra being switched
    public event EventHandler<BattleEventArgs> OnPlayerAttemptEscape; //Include Escape chance
    public event EventHandler<BattleEventArgs> OnTerraUseHeldItem; //Include Terra and Item
    public event EventHandler<BattleEventArgs> OnStartingCombat;
    public event EventHandler<TerraAttackDeclarationEventArgs> OnTerraAttackDeclaration; //Might not need
    public event EventHandler<TerraAttackParamsEventArgs> OnTerraDirectAttack;
    public event EventHandler<TerraDamageByTerraEventArgs> OnTerraDamageByTerra; //Might not need now that there is a damage calculation event
    public event EventHandler<BattleEventArgs> OnEnteringEndTurnState;

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

    private BattleType battleType;
    private Battlefield battlefield;
    private BattleStateManager battleStateManager;
    private BattleActionManager battleActionManager;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        battleType = BattleLoader.GetInstance().GetBattleType();

        SetupBattleScene();

        battlefield = new Battlefield(playerBattleObject.GetComponent<TerraParty>(), wildTerra.GetTerra());
        //Initialize the existing status conditions on the terra in the event system
        Terra playerLeadingTerra = battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra();
        Terra opponentLeadingTerra = battlefield.GetSecondaryBattleSide().GetTerraBattlePosition().GetTerra();
        playerLeadingTerra.GetStatusEffect()?.AddStatusEffectBattleActoin(this, playerLeadingTerra);
        opponentLeadingTerra.GetStatusEffect()?.AddStatusEffectBattleActoin(this, opponentLeadingTerra);

        battleActionManager = new BattleActionManager();
        battleStateManager = new BattleStateManager(this);

        UpdateTerraStatusBars();
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

    public void OpenMoveSelectionUI()
    {
        battleHUD.OpenMoveSelectionUI(playerBattleObject.GetComponent<TerraParty>().GetTerraList()[0].GetMoves());
    }

    public void MoveSelectionAction(int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        TerraMove selectedMove = battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null) {
            Debug.LogError("The move at index " + moveIndex + " is null");
            return;
        }
        if (selectedMove.GetCurrentPP() <= 0) {
            Debug.Log(BattleDialog.NoMovePowerPointsLeftMsg(selectedMove));
            return;
        }

        //Decrement the selected mvoes PP
        selectedMove.SetCurrentPP(selectedMove.GetCurrentPP() - 1);
        //Initializes the selected attack and add the new TerraAttack to the TerraAttackList
        TerraAttack selectedTerraAttack = new TerraAttack(
            battlefield.GetPrimaryBattleSide().GetTerraBattlePosition(),
            battlefield.GetSecondaryBattleSide().GetTerraBattlePosition(),
            selectedMove);
        selectedMove.GetMoveBase().AttackSelectionInit(selectedTerraAttack, this);
        battleActionManager.GetTerraAttackList().Add(selectedTerraAttack);

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

    public void EndBattle()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InvokeOnEnteringStartTurnState()
    {
        OnEnteringStartTurnState?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnEnteringActionSelectionState()
    {
        OnEnteringActionSelectionState?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnActionSelection()
    {
        OnActionSelection?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnEnteringCombatState()
    {
        OnEnteringCombatState?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnTerraSwitch()
    {
        OnTerraSwitch?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnPlayerAttemptEscape()
    {
        OnPlayerAttemptEscape?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnTerraUseHeldItem()
    {
        OnTerraUseHeldItem?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnStartingCombat()
    {
        OnStartingCombat?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnTerraAttackDeclaration(TerraAttack terraAttack)
    {
        OnTerraAttackDeclaration?.Invoke(this, new TerraAttackDeclarationEventArgs(terraAttack, this));
    }

    public void InvokeOnTerraDirectAttack(TerraAttackParams terraAttackParams)
    {
        OnTerraDirectAttack?.Invoke(this, new TerraAttackParamsEventArgs(terraAttackParams, this));
    }

    public TerraDamageByTerraEventArgs InvokeOnTerraDamageByTerra(TerraAttack terraAttack, TerraAttackLog terraAttackLog, int? damage)
    {
        TerraDamageByTerraEventArgs eventArgs = new TerraDamageByTerraEventArgs(terraAttack, terraAttackLog, damage, this);
        OnTerraDamageByTerra?.Invoke(this, eventArgs);

        return eventArgs;
    }

    public void InvokeOnEnteringEndTurnState()
    {
        OnEnteringEndTurnState?.Invoke(this, new BattleEventArgs(this));
    }

    public BattleHUD GetBattleHUD() { return battleHUD; }

    public BattleType GetBattleType() { return battleType; }

    public Battlefield GetBattlefield() { return battlefield; }

    public BattleActionManager GetBattleActionManager() { return battleActionManager; }
}
