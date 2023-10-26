using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossCharacter : MonoBehaviour
{
    [SerializeField] protected Transform DestinationPoint;
    [SerializeField] protected Transform SourcePoint;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isMoving;
    private bool TowardsDestination;
    public void Awake()
    {
        transform.position = SourcePoint.position;
        TowardsDestination = true;
    }
    public void Update()
    {
        Moving();
    }
    public void Moving()
    {
        if (!isMoving) return;
        if (TowardsDestination)
        {
            transform.position = Vector2.MoveTowards(transform.position, DestinationPoint.position, speed);
            if (Vector2.Distance(transform.position, DestinationPoint.position) < 0.12f)
            {
                isMoving = false;
                TowardsDestination = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, SourcePoint.position, speed);
            if (Vector2.Distance(transform.position, SourcePoint.position) < 0.12f)
            {
                isMoving = false;
                TowardsDestination = true;
            }
        }
    }
    public void setMoving(bool moving)
    {
        isMoving = moving;
    }
    public void Start()
    {
        DialogueState.instance.EndDialogue += Instance_EndDialogue;
        TimeManager.instance.ChangeSection += Instance_ChangeSection;
    }
    public void OnDestroy()
    {
        DialogueState.instance.EndDialogue -= Instance_EndDialogue;
        TimeManager.instance.ChangeSection -= Instance_ChangeSection;
    }

    private void Instance_ChangeSection()
    {
        isMoving = true;
    }


    private void Instance_EndDialogue()
    {
        isMoving = true;
    }
}
