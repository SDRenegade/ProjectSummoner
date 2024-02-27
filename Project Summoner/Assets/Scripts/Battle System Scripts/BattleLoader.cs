using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    private static BattleLoader instance;
    private List<Terra> primarySummonerTerraList;
    private List<Terra> secondarySummonerTerraList;
    private BattleType battleType;
    private BattleFormat battleFormat;

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
        primarySummonerTerraList = playerTerraList;
        secondarySummonerTerraList = new List<Terra> { wildTerra };
        battleType = BattleType.WILD;
        battleFormat = BattleFormat.DOUBLE;

        //Save the player/scene data before loading into the battle scene
        SaveSystem.GetInstance().SaveGame();

        SceneLoader.Load(SceneEnum.BattleScene);
    }

    public void LoadSummonerBattle() {}

    public void Clear()
    {
        primarySummonerTerraList = null;
        secondarySummonerTerraList = null;
    }

    public static BattleLoader GetInstance() { return instance; }

    public List<Terra> GetPrimarySummonerTerraList() { return primarySummonerTerraList; }

    public List<Terra> GetSecondarySummonerTerraList() { return secondarySummonerTerraList; }

    public BattleType GetBattleType() {  return battleType; }

    public BattleFormat GetBattleFormat() {  return battleFormat; }
}
