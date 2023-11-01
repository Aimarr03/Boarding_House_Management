using Ink;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueState : StateDefault
{

    [SerializeField] TextMeshProUGUI dialogueContent;
    [SerializeField] TextMeshProUGUI personName;
    [SerializeField] Image personImage;
    [SerializeField] Transform DialogueUI;

    [SerializeField] private SpriteRenderer CharacterSprite;
    [SerializeField] List<Transform> CharacterPosition;

    [SerializeField] private Transform Arrow;

    private DialogueTree currentStory;
    public InitialDialogueTree initialDialogueTree;
    private Dialogue_Starter_BigBoss currentCharacter;
    private string currentLine;
    public static DialogueState instance;
    private int conversationIndex;
    private int lineIndex;
    private DialogueTree.LineConversation currentLineConversation;

    //For Choice Manager
    [SerializeField] private Transform[] choicesUI;
    [SerializeField] private TextMeshProUGUI[] choicesText;

    public event Action EndDialogue;
    public event Action BoughtMoney;
    public enum CharacterSpritePosition
    {
        Left,
        Right
    }
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        DialogueUI.gameObject.SetActive(false);
        conversationIndex = 0;
        lineIndex = 0;
        Arrow.gameObject.SetActive(false);
    }
    public void OnDestroy()
    {
        Destroy(instance.gameObject);
    }
    public void Update()
    {
        if (GameManager.instance.GameIsPaused) return;
        if (Enabled)
        {
            OnClick();
        }
    }
    public override void EnterState()
    {
        base.EnterState();
        Enabled = true;
        TimeManager.instance.Pause();
        TimeManager.instance.StopBackgroundChange();
        if (GameManager.instance.gameStatus == GameManager.TypeOfGameStatus.Debt 
            || GameManager.instance.gameStatus == GameManager.TypeOfGameStatus.NoMoney)
        {
            TimeManager.instance.Pause();
            NormalState.instance.ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Enabled = false;
        if(currentStory.conversationType == DialogueTree.ConversationType.tax)
        {
            if (EconomyManager.instance.CheckMoney(currentStory.Cost))
            {
                EconomyManager.instance.UseMoney(currentStory.Cost);
                BoughtMoney?.Invoke();
            }
            if(currentStory.nextConversation == null)
            {
                Debug.Log("You Win!");
                GameManager.instance.SetGameStatus(GameManager.TypeOfGameStatus.Win);
            }
        }
        if(currentStory.conversationType == DialogueTree.ConversationType.Lose)
        {
            GameManager.instance.SetGameStatus(GameManager.TypeOfGameStatus.Debt);
            initialDialogueTree.dialogueTree = GameManager.instance.GetDialogueTree(GameManager.TypeOfGameStatus.Debt);
        }
        if(currentStory.conversationType == DialogueTree.ConversationType.GameOver)
        {
            GameManager.instance.ChangingScene(0);
        }
        DialogueUI.gameObject.SetActive(false);
        if(currentCharacter != null)
        {
            currentCharacter.SetCanDialogue(false);
        }
        ChangeDialogue();
        if (GameManager.instance.gameStatus == GameManager.TypeOfGameStatus.Debt
            || GameManager.instance.gameStatus == GameManager.TypeOfGameStatus.NoMoney)
        {
            TimeManager.instance.NormalSpeed();
            NormalState.instance.EnterState();
        }
        TimeManager.instance.NormalSpeed();
        TimeManager.instance.StartBackgroundChange();
        EndDialogue?.Invoke();
    }

    public override void OnClick()
    {
        
    }
    public void ContinueConversation()
    {
        if(Enabled)
        {
            if (dialogueContent.text == currentLine)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueContent.text = currentLine;
                Arrow.gameObject.SetActive(true);
            }
        }
    }

    public override void Hovering()
    {
        base.Hovering();
    }
    public void SetDialogue(DialogueTree dialgoue, int cost)
    {
        
    }
    public void AutoChangeDialogue()
    {
        if (currentStory.conversationType == DialogueTree.ConversationType.taxOpener)
        {
            if (EconomyManager.instance.CheckMoney(currentStory.Cost))
            {
                SetDialogue(currentStory.entireConversation[conversationIndex].choice.dialogueTree[0]);
            }
            else
            {
                SetDialogue(currentStory.entireConversation[conversationIndex].choice.dialogueTree[1]);
            }
        }
    }
    public void SetDialogue(DialogueTree dialogue, Dialogue_Starter_BigBoss characterTalking)
    {
        currentCharacter = characterTalking;
        SetDialogue(dialogue);
    }
    public void SetDialogue(DialogueTree dialogue)
    {
        currentStory = dialogue;
        dialogueContent.text = "";
        conversationIndex = 0;
        lineIndex = 0;
        currentLineConversation = currentStory.entireConversation[conversationIndex];
        currentLine = currentLineConversation.lines[lineIndex];
        DialogueUI.gameObject.SetActive(true);
        EnterState();
        StartConversation();
    }
    public void StartConversation()
    {
        StartCoroutine(DisplayDialogue());
        DisplayChoice();
        DisplayDetailDialogue();
    }
    public void NextLine()
    {
        if (lineIndex < currentLineConversation.lines.Length - 1)
        {
            lineIndex++;
            currentLine = currentLineConversation.lines[lineIndex];
            dialogueContent.text = "";  // Clear the text.
            DisplayDetailDialogue();
            StartCoroutine(DisplayDialogue());  // Start the coroutine to display the new line.
            DisplayChoice();
            Arrow.gameObject.SetActive(false);
        }
        else
        {
            NextLineConversation();
        }
    }
    public void DisplayDetailDialogue()
    {
        CharacterSO currentCharacterSO = currentLineConversation.characterSO;
        if (currentCharacter != null)
        {
            personName.text = currentCharacterSO.characterName;
            Sprite currentSprite = currentCharacterSO.GetSpriteExpression(currentLineConversation.expressionType);
            if (currentSprite != null)
            {
                personImage.sprite = currentSprite;
                personImage.color = Color.white;
            }
            else
            {
                personImage.color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            personName.text = "";
            personImage.color = new Color(1, 1, 1, 0);
        }
    }
    public void NextLineConversation()
    {
        if (conversationIndex < currentStory.entireConversation.Count - 1)
        {
            conversationIndex++;
            lineIndex = 0;
            dialogueContent.text = "";
            currentLineConversation = currentStory.entireConversation[conversationIndex];
            currentLine = currentLineConversation.lines[lineIndex];
            StartConversation();
        }
        else
        { 
            if(currentStory.conversationType == DialogueTree.ConversationType.taxOpener)
            {
                AutoChangeDialogue();
            }
            else
            {
                ExitState();
            }
        }
    }


    private IEnumerator DisplayDialogue()
    {
        foreach(char c in currentLine.ToCharArray())
        {
            dialogueContent.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void DisplayChoice()
    {
        DialogueTree.Choice currentChoice = currentLineConversation.choice;
        Debug.Log("Current Choice : "+ currentChoice.optionsToChoose.Count);
        if (currentChoice.optionsToChoose.Count>0)
        {
            for(int x = 0; x < choicesUI.Length; x++)
            {
                if (x < currentChoice.optionsToChoose.Count)
                {
                    Button currentButton = choicesUI[x].GetComponent<Button>();
                    if (x < currentChoice.dialogueTree.Count)
                    {
                        DialogueTree dialogueTree = currentChoice.dialogueTree[x];
                        currentButton.onClick.RemoveAllListeners();
                        currentButton.onClick.AddListener(() => SetDialogue(dialogueTree));
                    }
                    choicesUI[x].gameObject.SetActive(true);
                    choicesText[x].text = currentChoice.optionsToChoose[x];
                }
                else
                {
                    choicesUI[x].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach(Transform child in choicesUI)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void ChangeDialogue()
    {
        if(currentStory.conversationType == DialogueTree.ConversationType.tax || currentStory.CheckNextConversation())
        {
            currentCharacter.SetNewDialogue(currentStory.nextConversation);
        }
    }
    
}
