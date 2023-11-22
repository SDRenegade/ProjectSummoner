using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class FileDataHandler
{
    private string dataPath;
    private string fileName;

    public FileDataHandler(string dataPath, string fileName)
    {
        this.dataPath = dataPath;
        this.fileName = fileName;
    }

    public GamePersistentData Load()
    {
        string fullPath = Path.Combine(dataPath, fileName);
        GamePersistentData loadedData = null;
        if(File.Exists(fullPath)) {
            try {
                string dataToLoad;

                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GamePersistentData>(dataToLoad); 
            }
            catch(Exception e) {
                Debug.LogError("Error occured while loading data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GamePersistentData sceneSaveData)
    {
        string fullPath = Path.Combine(dataPath, fileName);

        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(sceneSaveData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError("Error occured while saving data to file: " + fullPath + "\n" + e);
        }
    }
}
