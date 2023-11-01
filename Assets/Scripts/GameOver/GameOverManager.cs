using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public InitialDialogueTree initialDialogueTree;
    public void Awake()
    {
        Debug.Log("Dialogue instance = "+DialogueState.instance == null);
        DialogueState.instance.SetDialogue(initialDialogueTree.dialogueTree);
    }
}
