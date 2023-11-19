using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossCharacter : MonoBehaviour, IDataPersistance
{
    [SerializeField] protected Transform DestinationPoint;
    [SerializeField] protected Transform SourcePoint;
    [SerializeField] protected float speed;
    private float defaultSpeed;
    [SerializeField] protected bool isMoving;
    [SerializeField] private Animator animator;
    [SerializeField] protected CharacterSO characterSO;
    [SerializeField] protected Dialogue_Starter_BigBoss bigBoss;
    [SerializeField] private List<DialogueTree> dialogueTrees;

    private bool TowardsDestination;
    private int indexWaiting;

    public void Awake()
    {
        transform.position = SourcePoint.position;
        TowardsDestination = true;
        indexWaiting = 0;
        isMoving = false;
        bigBoss.setInterract(false);
        bigBoss.SetCanDialogue(false);
        defaultSpeed = speed;
    }

    public void Update()
    {
        Moving();
    }

    public void Moving()
    {
        if (isMoving)
        {
            animator.SetBool("IsMoving", true);
            Transform targetPoint = TowardsDestination ? DestinationPoint : SourcePoint;
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed);
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.42f)
            {
                
                isMoving = false;
                animator.SetBool("IsMoving", false);
                TowardsDestination = !TowardsDestination; // Toggle the direction
                indexWaiting = 0;
                bigBoss.setInterract(true);
                bigBoss.SetCanDialogue(true);
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
        TimeManager.instance.ChangeDate += Instance_ChangeDate;
        TimeManager.instance.TimeChanged += Instance_TimeChanged;
    }

    private void Instance_TimeChanged(TimeManager.TimeState timestate)
    {
        switch (timestate)
        {
            case TimeManager.TimeState.pause:
                speed = 0f;
                break;
            case TimeManager.TimeState.normal: 
                speed = defaultSpeed; 
                break;
            case TimeManager.TimeState.fast:
                speed = defaultSpeed * 2; 
                break;
        }
    }

    private void Instance_ChangeDate()
    {
        indexWaiting++;
        if (bigBoss.GetDialogueTree() == null) return;
        if (!isMoving)
        {
            if (Vector2.Distance(transform.position, DestinationPoint.position) < 3f)
            {
                Debug.Log("Index Waiting " + indexWaiting);
                if (indexWaiting == 3)
                {
                    DialogueState.instance.SetDialogue(bigBoss.GetDialogueTree(), bigBoss);
                }
            }
        }
    }

    public void OnDestroy()
    {
        DialogueState.instance.EndDialogue -= Instance_EndDialogue;
        TimeManager.instance.ChangeSection -= Instance_ChangeSection;
        TimeManager.instance.ChangeDate -= Instance_ChangeDate;
        TimeManager.instance.TimeChanged -= Instance_TimeChanged;
    }

    private void Instance_ChangeSection()
    {
        if (GameManager.instance.gameStatus == GameManager.TypeOfGameStatus.Win) return;
        if (bigBoss.GetDialogueTree() == null) return;
        isMoving = true;
        Transform targetPoint = TowardsDestination ? DestinationPoint : SourcePoint;
        if(targetPoint.position == DestinationPoint.position)
        {
            transform.position = targetPoint.position;
        }
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public CharacterSO GetCharacterSO()
    {
        return characterSO;
    }
    private void Instance_EndDialogue()
    {
        isMoving = true;
        animator.SetBool("IsMoving", true);
        bigBoss.setInterract(false);
        bigBoss.SetCanDialogue(false);
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public void LoadScene(GameData gameData)
    {
        //Ditambah kan satu untuk menyesuaikan
        switch(gameData.Section)
        {
            case 0:
                bigBoss.SetNewDialogue(dialogueTrees[0]);
                break;
            case 1:
                bigBoss.SetNewDialogue(dialogueTrees[1]);
                break;    
            case 2:
                bigBoss.SetNewDialogue(dialogueTrees[2]);
                break;
            default:
                bigBoss.SetNewDialogue(null);
                break;
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        
    }
}
