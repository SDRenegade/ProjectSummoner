using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    private static BattleLoader instance;
    private List<Terra> primaryTerraList;
    private List<Terra> secondaryTerraList;
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
        primaryTerraList = playerTerraList;
        secondaryTerraList = new List<Terra> { wildTerra };
        battleType = BattleType.WILD;
        battleFormat = BattleFormat.DOUBLE;

        //Save the player/scene data before loading into the battle scene
        SaveSystem.GetInstance().SaveGame();

        SceneLoader.Load(SceneEnum.BattleScene);
    }

    public void LoadSummonerBattle() {}

    public void Clear()
    {
        primaryTerraList = null;
        secondaryTerraList = null;
    }

    public static BattleLoader GetInstance() { return instance; }

    public List<Terra> GetPrimaryTerraList() { return primaryTerraList; }

    public List<Terra> GetSecondaryTerraList() { return secondaryTerraList; }

    public BattleType GetBattleType() {  return battleType; }

    public BattleFormat GetBattleFormat() {  return battleFormat; }
}
