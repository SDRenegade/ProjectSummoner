using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;

    [SerializeField] private string fileName;
    private FileDataHandler fileDataHandler;
    private GamePersistentData gamePersistentData;
    private List<IPersistentData> persistentDataObjects;

    private void Awake()
    {
        //Checks if a SaveSystem component is already in the scene. If another SaveSystem component is found
        //this instance of the SaveSystem is then redundant and is destroyed.
        if (instance != null) {
            Debug.Log("Another SaveSystem component is already in the scene. Destroying this game object.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    //Subscribes to the OnSceneLoaded and OnSceneUnloaded delegates to listen for when scenes change
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Unsubscribes from the OnSceneLoaded and OnSceneUnloaded delegates when the game object this script is
    //attached to is disabled
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Saves the game data when the game is closed
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //Finds all ISaveData objects in the scene and loads their data when a new scene is loaded
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        persistentDataObjects = FindAllSaveDataObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gamePersistentData = new GamePersistentData();
    }

    public void LoadGame()
    {
        gamePersistentData = fileDataHandler.Load();
        if (gamePersistentData == null) {
            Debug.Log("No persistent game data was found. Initializing default scene data");
            NewGame();
        }
        else {
            foreach (IPersistentData saveDataObj in persistentDataObjects)
                saveDataObj.LoadData(gamePersistentData);
        }
    }

    public void SaveGame()
    {
        foreach (IPersistentData saveDataObj in persistentDataObjects)
            saveDataObj.SaveData(ref gamePersistentData);
        fileDataHandler.Save(gamePersistentData);
    }

    //Finds all the objects in the scene that have the ISaveData interface so the save and load methods
    //can iterate through the objects when needed
    public List<IPersistentData> FindAllSaveDataObjects()
    {
        IEnumerable<IPersistentData> saveDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistentData>();

        return new List<IPersistentData>(saveDataObjects);
    }

    public static SaveSystem GetInstance() { return instance; }

    public GamePersistentData GetGamePersistentData() { return gamePersistentData; }
}
