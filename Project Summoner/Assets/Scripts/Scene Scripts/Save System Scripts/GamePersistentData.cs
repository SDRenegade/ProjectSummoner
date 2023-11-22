using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class GamePersistentData
{
    [SerializeField] private PlayerSaveData playerSaveData;
    //[SerializeField] private SerializableDictionary<string, ScenePersistentData> scenePersistentDataMap;

    public GamePersistentData()
    {
        playerSaveData = new PlayerSaveData();
        /*scenePersistentDataMap = new SerializableDictionary<string, ScenePersistentData>();
        foreach(SceneEnum scene in System.Enum.GetValues(typeof(SceneEnum)))
            scenePersistentDataMap.Add(scene.ToString(), new ScenePersistentData());*/
    }

    public PlayerSaveData GetPlayerSaveData() { return playerSaveData; }

    public void SetPlayerSaveData(PlayerSaveData playerSaveData) { this.playerSaveData = playerSaveData; }

    //public Dictionary<string, ScenePersistentData> GetScenePersistentDataMap() { return scenePersistentDataMap; }

    //public void SetScenePersistentDataMap(SerializableDictionary<string, ScenePersistentData> scenePersistentDataMap) { this.scenePersistentDataMap = scenePersistentDataMap; }
}
