using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    private static BattleLoader instance;
    private BattleType battleType;
    private BattleFormat battleFormat;
    private List<Terra> primaryTerraList;
    private List<Terra> secondaryTerraList;

    private void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadWildBattle(List<Terra> playerTerraList, List<Terra> wildTerraList)
    {
        battleType = BattleType.Summoner;
        battleFormat = BattleFormat.DOUBLE;
        primaryTerraList = playerTerraList;
        if(wildTerraList.Count >= battleFormat.NumberOfLeadingPositions()) {
            for(int i = wildTerraList.Count - 1; i >= battleFormat.NumberOfLeadingPositions(); i--)
                wildTerraList.RemoveAt(i);
        }
        secondaryTerraList = wildTerraList;

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
