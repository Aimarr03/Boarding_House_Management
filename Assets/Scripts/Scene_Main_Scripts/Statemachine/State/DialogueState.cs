using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueState : StateDefault
{
    [SerializeField] TextMeshProUGUI dialogueContent;
    [SerializeField] TextMeshProUGUI personName;
    [SerializeField] Transform DialogueUI;

    private Story currentStory;
    private Dialogue_Starter_BigBoss currentCharacter;
    private string currentLine;
    public static DialogueState instance;

    public event Action EndDialogue;
    public event Action BoughtMoney;
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        DialogueUI.gameObject.SetActive(false);
    }
    public void Update()
    {
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
        NormalState.instance.ExitState();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (currentStory.variablesState["CheckMoney"] != null)
        {
            int cost = (int)currentStory.variablesState["MoneyCost"];
            EconomyManager.instance.UseMoney(cost);
            BoughtMoney?.Invoke();
        }
        Enabled = false;
        DialogueUI.gameObject.SetActive(false);
        currentCharacter.SetCanDialogue(false);
        TimeManager.instance.NormalSpeed();
        NormalState.instance.EnterState();
        EndDialogue?.Invoke();
    }

    public override void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogueContent.text == currentLine)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueContent.text = currentLine;
            }
        }
    }

    public override void Hovering()
    {
        base.Hovering();
    }
    public void SetDialogue(TextAsset dialogue, Dialogue_Starter_BigBoss characterTalking)
    {
        currentStory = new Story(dialogue.text);
        currentCharacter = characterTalking;
        dialogueContent.text = "";
        DialogueUI.gameObject.SetActive(true);
        if (currentStory.variablesState["CheckMoney"] != null)
        {
            int cost = (int)currentStory.variablesState["MoneyCost"];
            currentStory.variablesState["CheckMoney"] = EconomyManager.instance.CheckMoney(cost);
        }
        EnterState();
        currentLine = currentStory.Continue();
        StartCoroutine(DisplayDialogue());
    }
    public void StartConversation()
    {
        StartCoroutine(DisplayDialogue());
    }
    public void NextLine()
    {
        if (currentStory.canContinue)
        {
            currentLine = currentStory.Continue();  // Set the new line first.
            dialogueContent.text = "";  // Clear the text.
            StartCoroutine(DisplayDialogue());  // Start the coroutine to display the new line.
        }
        else
        {
            ExitState();
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

    
}
