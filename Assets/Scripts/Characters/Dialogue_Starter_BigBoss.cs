using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Starter_BigBoss : InterractableObject
{
    [SerializeField] private DialogueTree Dialogue;
    [SerializeField] private bool canDialogue;
    [SerializeField] private BigBossCharacter character;
    public void Start()
    {

    }
    public override void Interracted()
    {
        base.Interracted();
        if (canDialogue && Dialogue != null)
        { 
            DialogueState.instance.SetDialogue(Dialogue, this);
        }
    }
    public void SetCanDialogue(bool input)
    {
        canDialogue = input;
    }
    public void SetNewDialogue(DialogueTree dialogueTree)
    {
        Dialogue = dialogueTree;
    }
    public DialogueTree GetDialogueTree()
    {
        return Dialogue;
    }
}
