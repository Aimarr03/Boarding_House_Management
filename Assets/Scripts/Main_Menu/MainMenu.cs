using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<Button> buttonList;
    public Button loadButton;
    public void Start()
    {
        if (!DataPersistanceManager.instance.HasGameData())
        {
            loadButton.interactable = false;
        }
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        OnDisable();
        Debug.Log("Start New Game");
        DataPersistanceManager.instance.NewGame();

        SceneManager.LoadSceneAsync(1);
    }
    public void LoadGame()
    {
        OnDisable();
        Debug.Log("Continue Game");
        
        SceneManager.LoadSceneAsync(1);
    }
    public void OnDisable()
    {
        foreach(Button button in buttonList)
        {
            button.interactable = false;
        }
    }
}
