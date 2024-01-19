using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    private static BattleLoader instance;
    private GameObject player;
    private Terra wildTerra;
    private BattleType battleType;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
            Destroy(this.gameObject);
    }

    public void LoadWildBattle(GameObject player, Terra wildTerra)
    {
        this.player = player;
        this.wildTerra = wildTerra;
        battleType = BattleType.WILD;

        //StoreMainSceneData();

        DontDestroyOnLoad(player);

        SceneLoader.Load(SceneEnum.BattleScene);
    }

    public void LoadSummonerBattle() {}

    public void Clear()
    {
        player = null;
        wildTerra = null;
    }

    public static BattleLoader GetInstance() { return instance; }

    public GameObject GetPlayer() { return player; }

    public Terra GetWildTerra() { return wildTerra; }

    public BattleType GetBattleType() {  return battleType; }

}
