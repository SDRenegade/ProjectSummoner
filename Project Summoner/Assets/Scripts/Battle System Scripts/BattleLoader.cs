using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    private static BattleLoader instance;
    private List<Terra> playerTerraList;
    private Terra wildTerra;
    private BattleType battleType;

    private void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadWildBattle(List<Terra> playerTerraList, Terra wildTerra)
    {
        this.playerTerraList = playerTerraList;
        this.wildTerra = wildTerra;
        battleType = BattleType.WILD;

        //Save the player/scene data before loading into the battle scene
        SaveSystem.GetInstance().SaveGame();

        SceneLoader.Load(SceneEnum.BattleScene);
    }

    public void LoadSummonerBattle() {}

    public void Clear()
    {
        playerTerraList = null;
        wildTerra = null;
    }

    public static BattleLoader GetInstance() { return instance; }

    public List<Terra> GetPlayerTerraList() { return playerTerraList; }

    public Terra GetWildTerra() { return wildTerra; }

    public BattleType GetBattleType() {  return battleType; }

}
