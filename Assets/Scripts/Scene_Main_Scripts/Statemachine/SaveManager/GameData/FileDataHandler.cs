using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class FileDataHandler
{
    public string directoryPath;
    public string fileName;

    public FileDataHandler(string directoryPath, string fileName)
    {
        this.directoryPath = directoryPath;
        this.fileName = fileName;
    }

    public void SaveData(GameData gameData)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, Path.Combine(directoryPath, fileName));
        
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(fullPath, data);
    }

    public GameData LoadData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, Path.Combine(directoryPath, fileName));
        if (File.Exists(fullPath))
        {
            try
            {
                string data = File.ReadAllText(fullPath);
                GameData gameData = JsonUtility.FromJson<GameData>(data);
                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return null;
    }
    
}
