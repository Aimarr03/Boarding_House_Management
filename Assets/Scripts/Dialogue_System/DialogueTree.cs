using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Conversation", menuName ="Create new Dialogue Tree" )]
public class DialogueTree: ScriptableObject
{
    public string DialogueName;
    public List<CharacterSO> charactersInvolved;
    public List<LineConversation> entireConversation;
    public bool ConversationDone;
    public ConversationType conversationType;
    public int Cost;
    public DialogueTree nextConversation;
    public enum ConversationType
    {
        Opening,
        taxOpener,
        tax,
        Lose,
        normal,
        repair,
        tutorial,
        GameOver
    }
    [System.Serializable]
    public class LineConversation
    {
        public CharacterSO characterSO;
        public CharacterSO.ExpressionType expressionType;
        public DialogueState.CharacterSpritePosition position;
        [TextArea(3,10)]
        public string[] lines;
        public Choice choice;

        public bool CheckHasChoice()
        {
            return choice != null;
        }
    }
    public bool CheckNextConversation()
    {
        return nextConversation != null;
    }

    [System.Serializable]
    public class Choice
    {
        public List<string> optionsToChoose;
        public List<DialogueTree> dialogueTree;
    }

}
