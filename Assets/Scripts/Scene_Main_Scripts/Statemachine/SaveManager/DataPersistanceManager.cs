using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DataPersistanceManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private string directoryPath;

    private FileDataHandler fileHandler;
    public GameData gameData;
    public static DataPersistanceManager instance { get; private set; }
    List<IDataPersistance> ListDataPersistance;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is one more! Destroy newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this; 
        ListDataPersistance = new List<IDataPersistance>();
        fileHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        DontDestroyOnLoad(this.gameObject);   
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ListDataPersistance = GetListDataPersistance();
        LoadGame();
    }

    private List<IDataPersistance> GetListDataPersistance()
    {
        IEnumerable<IDataPersistance> ListDataPersistance = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(ListDataPersistance);
    }

    public void NewGame()
    {
        gameData = new GameData();
        fileHandler.NewData();
    }
    public void LoadGame()
    {
        gameData = fileHandler.LoadData();
        if(gameData == null)
        {
            Debug.LogWarning("No Data was Found!");
            NewGame();
        }
        foreach(IDataPersistance iDataPersistance in  ListDataPersistance)
        {
            iDataPersistance.LoadScene(gameData);
        }
    }
    public void SaveGame()
    {
        if(gameData == null)
        {
            Debug.LogWarning("No data was found!");
            return;
        }
        foreach (IDataPersistance iDataPersistance in ListDataPersistance)
        {
            iDataPersistance.SaveScene(ref gameData);
        }
        fileHandler.SaveData(gameData);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
}
