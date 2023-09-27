using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractableDialogue : Base_Interactable_Objects
{
    public override void Interract()
    {
        base.Interract();
        Debug.Log("Hey there you have interact with this objects!");
    }
}
