using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ManagerCharacter : MonoBehaviour
{
    [SerializeField] private Transform defaultPosition;
    public static ManagerCharacter instance;
    [SerializeField] private Animator animator;
    public bool isBusy;
    public void Awake()
    {
        if(instance != null) return; 
        instance = this;
        isBusy = false;
    }
    public IEnumerator IEnumeratorAction(InterractableObject action)
    {
        if(action is BrokenIndicator)
        {
            Debug.Log("Broken Indicator");
            animator.SetBool("IsCleaning", true);
            isBusy = true;
            BrokenIndicator brokenIndicator = action as BrokenIndicator;
            transform.position = brokenIndicator.repairingPosition.position;
            
            yield return new WaitForSeconds(3f);
            isBusy =false;
            StopAction();
        }
        if(action is Cleaning && !isBusy)
        {
            Debug.Log("Cleaning");
            animator.SetBool("IsCleaning", true);
            Cleaning cleaning = action as Cleaning;
            transform.position = new Vector2(cleaning.cleaningPosition.position.x, transform.position.y);
            yield return new WaitForSeconds(cleaning._durationCleaning);
            StopAction();
        }
        
    }
    public void DoAction(InterractableObject action)
    {
        StartCoroutine(IEnumeratorAction(action));
        Debug.Log("Character Position: " + transform.position);
        Debug.Log("Busy : " + IsBusy());
    }
    public void StopAction()
    {
        Debug.Log("Stop Action");
        StopAllCoroutines();
        transform.position = defaultPosition.position;
        animator.SetBool("IsCleaning", false);
    }
    //Check if position is not equal to default position, meaning it is busy
    public bool IsBusy()
    {
        return transform.position != defaultPosition.position;
    }
    public void SetPosition(Transform position)
    {
        this.defaultPosition = position;
        transform.position = position.position;
    }
}
