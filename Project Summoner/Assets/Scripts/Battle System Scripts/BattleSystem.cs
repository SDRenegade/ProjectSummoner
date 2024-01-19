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
    public event EventHandler<BattleEventArgs> OnStartTurn;
    public event EventHandler<BattleEventArgs> OnActionStep;
    public event EventHandler<BattleEventArgs> OnTerraSwitch; //Include BattleSide and the two Terra being switched
    public event EventHandler<BattleEventArgs> OnPlayerAttemptEscape; //Include Escape chance
    public event EventHandler<BattleEventArgs> OnTerraUseHeldItem; //Include Terra and Item
    public event EventHandler<TerraAttackTerraEventArgs> OnTerraAttackTerra;
    public event EventHandler<TerraAttackTerraEventArgs> OnTerraPostAttack;
    public event EventHandler<BattleEventArgs> OnEndTurn;

    //UI elements
    [SerializeField] private TerraBattleStatusBar playerTerraStatusBar;
    [SerializeField] private TerraBattleStatusBar opponentTerraStatusBar;
    [SerializeField] private GameObject menuSelectionUIGO;
    [SerializeField] private GameObject moveSelectionUIGO;

    //Game Object positions
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform opponentTransform;
    [SerializeField] private Transform playerTerraTransform;
    [SerializeField] private Transform opponentTerraTransform;

    //Player & opposing summoner prefabs
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject opponentPrefab;

    //Player & opposing summoner Game Objects
    private GameObject playerGO;
    //private GameObject opponentGO

    //The currently battling Terras and their gameobject (Change the neme of the variables later)
    private TerraBattleObject playerLeadingTerra;
    private TerraBattleObject wildTerra;

    private Battlefield battlefield;
    private BattleStateManager battleStateManager;

    private BattleType battleType;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        battleType = BattleLoader.GetInstance().GetBattleType();

        InitializeBattleScene();

        battlefield = new Battlefield(playerGO.GetComponent<TerraParty>(), wildTerra.GetTerra());
        //Initialize the existing status conditions on the terra in the event system
        if (battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetStatusEffect() != null)
            battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetStatusEffect().AddBattleEvent(this);
        if (battlefield.GetSecondaryBattleSide().GetTerraBattlePosition().GetTerra().GetStatusEffect() != null)
            battlefield.GetSecondaryBattleSide().GetTerraBattlePosition().GetTerra().GetStatusEffect().AddBattleEvent(this);
        battleStateManager = new BattleStateManager(this);

        UpdateTerraStatusBars();
    }

    private void InitializeBattleScene()
    {
        CloseAllSelectionUI();

        if (BattleLoader.GetInstance().GetPlayer() != null) {
            playerGO = Instantiate(playerPrefab);
            playerGO.transform.position = playerTransform.position;
            playerGO.transform.eulerAngles = playerTransform.eulerAngles;
            TerraParty playerParty = playerGO.GetComponent<TerraParty>();
            playerParty.CopyTerraParty(BattleLoader.GetInstance().GetPlayer().GetComponent<TerraParty>());
            Destroy(BattleLoader.GetInstance().GetPlayer());

            playerLeadingTerra = new TerraBattleObject(playerParty.GetTerraList()[0]);
            playerLeadingTerra.SetTerraGO(Instantiate(playerLeadingTerra.GetTerra().GetTerraBase().GetTerraGameObject()));
            playerLeadingTerra.GetTerraGO().transform.position = playerTerraTransform.position;
            playerLeadingTerra.GetTerraGO().transform.eulerAngles = playerTerraTransform.eulerAngles;
        }
        else
            Debug.Log("No player object detected in BattleLoader");

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
        playerTerraStatusBar.UpdateStatusBar(battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra());
        opponentTerraStatusBar.UpdateStatusBar(battlefield.GetSecondaryBattleSide().GetTerraBattlePosition().GetTerra());
    }

    public void OpenMenuSelectionUI()
    {
        moveSelectionUIGO.SetActive(false);
        menuSelectionUIGO.SetActive(true);
    }

    public void OpenMoveSelectionUI()
    {
        menuSelectionUIGO.SetActive(false);

        MoveSelectionUI moveSelectionUI = moveSelectionUIGO.GetComponent<MoveSelectionUI>();
        List<TerraMove> terraMoves = playerGO.GetComponent<TerraParty>().GetTerraList()[0].GetMoves();
        for (int i = 0; i < Terra.MOVE_SLOTS; i++) {
            if (moveSelectionUI.GetMoveBtns().Length <= i)
                break;

            string moveBtnName = MoveSelectionUI.EMPTY_SLOT_NAME;
            if (i < terraMoves.Count && terraMoves[i] != null)
                moveBtnName = terraMoves[i].GetMoveBase().GetMoveName() + " " + terraMoves[i].GetCurrentPP() + "/" + terraMoves[i].GetMaxPP();
            moveSelectionUI.GetMoveBtns()[i].GetComponentInChildren<TextMeshProUGUI>().SetText(moveBtnName);
        }
        this.moveSelectionUIGO.SetActive(true);
    }

    public void MoveSelectionAction(int moveIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        TerraMove selectedMove = battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetMoves()[moveIndex];
        if (selectedMove == null) {
            Debug.Log("The move at index " + moveIndex + " is null");
            return;
        }
        if (battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetMoves()[moveIndex].GetCurrentPP() <= 0) {
            Debug.Log(selectedMove.GetMoveBase().GetMoveName() + " has no PP left");
            return;
        }

        battlefield.GetTerraAttackList().Add(new TerraAttack(
            battlefield.GetPrimaryBattleSide().GetTerraBattlePosition(),
            battlefield.GetSecondaryBattleSide().GetTerraBattlePosition(),
            battlefield.GetPrimaryBattleSide().GetTerraBattlePosition().GetTerra().GetMoves()[moveIndex]));
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void UseSelectedItemAction(int itemIndex)
    {
        if (battleStateManager.GetCurrentState() != battleStateManager.GetActionSelectionState())
            return;

        //Add item to the queued item list
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void SwitchLeadingTerraAction(int switchingTerraPartyIndex)
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
            Debug.Log("You cannot flee from a summoner battle");
            return;
        }

        //Escape Logic
        battleStateManager.SwitchState(battleStateManager.GetCombatState());
    }

    public void CloseAllSelectionUI()
    {
        moveSelectionUIGO.SetActive(false);
        menuSelectionUIGO.SetActive(false);
    }

    public void EndBattle()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InvokeOnStartTurn()
    {
        OnStartTurn?.Invoke(this, new BattleEventArgs(this));
    }

    public void InvokeOnActionStep()
    {
        OnActionStep?.Invoke(this, new BattleEventArgs(this));
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

    public void InvokeOnTerraAttackTerra(TerraAttack terraAttack)
    {
        OnTerraAttackTerra?.Invoke(this, new TerraAttackTerraEventArgs(terraAttack, this));
    }

    public void InvokeOnTerraPostAttack(TerraAttack terraAttack)
    {
        OnTerraPostAttack?.Invoke(this, new TerraAttackTerraEventArgs(terraAttack, this));
    }

    public void InvokeOnEndTurn()
    {
        OnEndTurn?.Invoke(this, new BattleEventArgs(this));
    }

    public BattleType GetBattleType() { return battleType; }

    public Battlefield GetBattlefield() { return battlefield; }
}
