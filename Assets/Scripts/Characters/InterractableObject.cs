using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool canInterract;
    [SerializeField] protected Transform InterractedIndicator;

    public virtual void Awake()
    {
        canInterract = true;
    }

    public virtual void Interracted()
    {
        if (canInterract)
        {
            Debug.Log("Interracted!");
        }
        else
        {
            Debug.Log("Cannot Interracted!");
        }
    }
    public virtual void ExitInterracted()
    {

    }
    public virtual void HoldInterraction()
    {

    }
    public void setInterract(bool input)
    {
        canInterract = input;
    }
    public bool GetInterractState()
    {
        return canInterract;
    }
    public virtual void OnHoverEnter()
    {
        Debug.Log("Hovering");
        InterractedIndicator.gameObject.SetActive(true);
    }

    public virtual void OnHoverExit()
    {
        Debug.Log("Exit");
        InterractedIndicator.gameObject.SetActive(false);
    }
}
