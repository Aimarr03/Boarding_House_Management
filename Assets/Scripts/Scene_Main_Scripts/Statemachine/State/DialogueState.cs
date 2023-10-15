using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueState : StateDefault
{
    [SerializeField] TextMeshProUGUI dialogueContent;
    [SerializeField] TextMeshProUGUI personName;
    [SerializeField] Transform DialogueUI;

    private Dialogue_Component currentDialogue;
    private List<Dialogue_Component.Dialogue> currentConversation;
    private Dialogue_Starter currentCharacter;
    private int conversationIndex;
    public static DialogueState instance;

    public void Awake()
    {
        if (instance != null) return;
        instance = this;
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
    }

    public override void ExitState()
    {
        base.ExitState();
        Enabled = false;
        DialogueUI.gameObject.SetActive(false);
    }

    public override void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogueContent.text == currentConversation[conversationIndex].dialogue)
            {
                NextLine();
            }
            else
            {
                dialogueContent.text = currentConversation[conversationIndex].dialogue;
            }
        }
    }

    public override void Hovering()
    {
        base.Hovering();
    }
    public void SetDialogue(Dialogue_Component dialogue, Dialogue_Starter characterTalking)
    {
        currentDialogue = dialogue;
        currentCharacter = characterTalking;
        currentConversation = currentDialogue.Conversation;
        dialogueContent.text = "";
        conversationIndex = 0;
        EnterState();
        StartCoroutine(DisplayDialogue());
    }
    public void StartConversation()
    {
        StartCoroutine(DisplayDialogue());
    }
    public void NextLine()
    {
        if(conversationIndex < currentConversation.Count-1)
        {
            conversationIndex++;
            dialogueContent.text = "";
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            currentDialogue.ConversationDone = true;
            currentCharacter.setInterract(false);
            ExitState();
            NormalState.instance.EnterState();
        }
    }
    private IEnumerator DisplayDialogue()
    {
        foreach(char c in currentConversation[conversationIndex].dialogue)
        {
            dialogueContent.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    
}
