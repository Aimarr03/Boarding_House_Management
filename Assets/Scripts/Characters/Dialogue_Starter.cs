using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Starter : InterractableObject
{
    [SerializeField]private Dialogue_Component dialogueComponent;
    public void Start()
    {

    }
    public void SetDialogueComponent(Dialogue_Component dialogueComponent)
    {
        this.dialogueComponent = dialogueComponent;
    }
    public override void Interracted()
    {
        base.Interracted();
        if (!dialogueComponent.ConversationDone)
        {
            NormalState.instance.ExitState();
            DialogueState.instance.SetDialogue(dialogueComponent, this);
        }
    }

}
