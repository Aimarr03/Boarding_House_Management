using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : ScriptableObject
{
    public string id_Dialogue;
    public string characterName;
    public string DialogueLine;
    public DialogueType type;
    public position characterPosition;
    public enum DialogueType
    {
        Normal,
        Choice
    }
    public enum position
    {
        left,
        right
    }
}
