using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Interactable_Objects : MonoBehaviour, I_Interactable
{
    [SerializeField] protected string playerTag;
    [SerializeField] protected Transform TransformOutline;

    public void Awake()
    {
        TransformOutline.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Interract");
        if (collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Interract Player");
            TransformOutline.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        TransformOutline.gameObject.SetActive(false);
    }
    public virtual void Interract()
    {
        
    }
}
