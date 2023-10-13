using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacter : MonoBehaviour
{
    [SerializeField] InterractableObject interractableObject;
    public void Awake()
    {
        NormalState.OnHovering += GameplayScene_OnHovering;
    }

    private void GameplayScene_OnHovering(InterractableObject currentInterractedObject)
    {
        if(currentInterractedObject == interractableObject)
        {
            interractableObject.OnHoverEnter();
        }
        else
        {
            interractableObject.OnHoverExit();
        }
    }
}
