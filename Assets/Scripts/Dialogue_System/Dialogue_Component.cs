using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new_conversation", menuName="create conversation")]
public class Dialogue_Component : ScriptableObject
{
    public List<string> characters_Involved;
    public bool ConversationDone;
    public List<Dialogue> Conversation;
}
