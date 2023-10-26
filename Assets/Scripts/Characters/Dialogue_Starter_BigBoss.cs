using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Starter_BigBoss : InterractableObject
{
    [SerializeField] private TextAsset Dialogue;
    [SerializeField] private bool canDialogue;
    [SerializeField] private BigBossCharacter character;
    public void Start()
    {

    }
    public void SetDialogueComponent(Dialogue_Component dialogueComponent)
    {
        
    }
    public override void Interracted()
    {
        base.Interracted();
        if (canDialogue && Dialogue != null)
        {
            Debug.Log(Dialogue.text);
            DialogueState.instance.SetDialogue(Dialogue, this);
        }
    }
    public void SetCanDialogue(bool input)
    {
        canDialogue = input;
    }

}
