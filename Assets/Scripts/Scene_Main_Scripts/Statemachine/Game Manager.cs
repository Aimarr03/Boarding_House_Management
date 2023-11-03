using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistance
{
    public static GameManager instance;
    public bool GameOverStatus;
    public bool GameIsPaused;
    public List<CustomDictionary> TypeOfDialogue;
    public TypeOfGameStatus gameStatus;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private CanvasGroup headerGroup;
    [SerializeField] private Transform PauseUI;

    [System.Serializable]
    public class CustomDictionary
    {
        public TypeOfGameStatus typeOfGameStatus;
        public DialogueTree dialogueTree;
    }
    public void Start()
    {
        if(gameStatus == TypeOfGameStatus.NoMoney || gameStatus == TypeOfGameStatus.Debt)
        {
            StartCoroutine(FadeTo(20f,  5f));
        }
    }
    public DialogueTree GetDialogueTree(TypeOfGameStatus typeOfGameStatus)
    {
        foreach(CustomDictionary customDictionary in TypeOfDialogue)
        {
            if(typeOfGameStatus == customDictionary.typeOfGameStatus)
            {
                return customDictionary.dialogueTree;
            }
        }
        return null;
    }
    public enum TypeOfGameStatus
    {
        Normal,
        Win,
        Debt,
        NoMoney
    }
    public void Awake()
    {
        if (instance != null) return;
        instance = this; 
        GameOverStatus = false;
        GameIsPaused = false;
        blackScreen.SetActive(false);
        PauseUI.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }    
    }
    public void SetGameStatus(TypeOfGameStatus input)
    {
        if(input != TypeOfGameStatus.Win)
        {
            GameOverStatus = true;
            StartCoroutine(FadeTo(20f, 5f));
        }
        gameStatus = input;
    }
    public IEnumerator FadeTo(float aValue, float aTime)
    {
        blackScreen.gameObject.SetActive(true);
        Image blackScreenImage = blackScreen.GetComponent<Image>();
        float alphaBlackScreen = blackScreenImage.color.a;
        for(float t = 0; t < 1.0; t += Time.deltaTime / aTime)
        {
            float alpha = Mathf.Lerp(1, 0, 2);
            headerGroup.alpha = alpha;
            Color newColor = new Color(0,0,0, Mathf.Lerp(alphaBlackScreen, aValue, t));
            blackScreenImage.color = newColor;
            yield return null;
        }
        Debug.Log(GetDialogueTree(gameStatus));
        DialogueState.instance.SetDialogue(GetDialogueTree(gameStatus));
    }
    public void ChangingScene(int input)
    {
        DataPersistanceManager.instance.SaveGame();
        StartCoroutine(LoadScene(input));
        Debug.Log("Data Saved");
    }
    public IEnumerator LoadScene(int scene)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(0);
    }
    public void PauseGame()
    {
        GameIsPaused = true;
        TimeManager.instance.Pause();
        TimeManager.instance.StopBackgroundChange();
        PauseUI.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        GameIsPaused = false;
        TimeManager.instance.NormalSpeed();
        TimeManager.instance.StartBackgroundChange();
        PauseUI.gameObject.SetActive(false);
    }

    public void LoadScene(GameData gameData)
    {
        gameStatus = gameData.gameState.GetGameStatus();
    }

    public void SaveScene(ref GameData gameData)
    {
        gameData.gameState.SetString(gameStatus);
    }
    public void SaveGameData()
    {
        DataPersistanceManager.instance.SaveGame();
        Debug.Log("Data Saved");
    }
}
