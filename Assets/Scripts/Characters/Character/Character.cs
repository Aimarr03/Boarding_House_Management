using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterSO characterSO;
    [SerializeField] private Dialogue_Starter_Character _CharacterInterraction;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float _movementSpeed;
    private float defaultSpeed;
    [SerializeField] CharacterMoodIndicator moodIndicator;
    public int waitingDay;
    
    private RoomSlot roomSlot;
    private MoodIndicator currentMood;
    public enum MoodIndicator
    {
        Happy,
        Normal,
        Angry,
        Dissapointed
    }
    public void Awake()
    {
        waitingDay = 0;
        targetPosition = transform.position;
        defaultSpeed = _movementSpeed;
    }
    public void Start()
    {
        ChangeMood(MoodIndicator.Normal);
        TimeManager.instance.TimeChanged += Instance_TimeChanged;
        switch (TimeManager.instance.currentTimeState)
        {
            case TimeManager.TimeState.pause:
                _movementSpeed = 0;
                break;
            case TimeManager.TimeState.normal:
                _movementSpeed = defaultSpeed;
                break;
            case TimeManager.TimeState.fast:
                _movementSpeed = defaultSpeed * 2;
                break;
        }
    }
    public void OnDestroy()
    {
        TimeManager.instance.TimeChanged -= Instance_TimeChanged;   
    }

    private void Instance_TimeChanged(TimeManager.TimeState timeState)
    {
        switch (timeState)
        {
            case TimeManager.TimeState.pause:
                _movementSpeed = 0;
                break;
            case TimeManager.TimeState.normal:
                _movementSpeed = defaultSpeed; 
                break;
            case TimeManager.TimeState.fast:
                _movementSpeed = defaultSpeed * 2;
                break;
        }
    }

    

    public void MoveTo()
    {
        Vector2 PositionVector2 = transform.position;
        if(PositionVector2 != targetPosition)
        {
            transform.position = Vector2.MoveTowards(PositionVector2, targetPosition, _movementSpeed);
            animator.SetBool("IsMoving", true);
            _CharacterInterraction.setInterract(false);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            _CharacterInterraction.setInterract(true);
        }
    }
    public bool IsOccupied()
    {
        return roomSlot != null;
    }
    public void SetRoomSlot(RoomSlot roomSlot)
    {
        this.roomSlot = roomSlot;
        waitingDay = 0;
        ChangeMood(MoodIndicator.Normal) ;
    }
    public void ChangeMood(MoodIndicator moodChange)
    {
        currentMood = moodChange;
        moodIndicator.SetMoodIndicator(currentMood);
        if(moodChange == MoodIndicator.Dissapointed)
        {
            Destroy(gameObject);
        }
    }
    public MoodIndicator GetCurrentMood()
    {
        return currentMood;
    }
    public float GetMultiplyMoodIndicator()
    {
        switch (currentMood)
        {
            case MoodIndicator.Happy:
                return 1.5f;
            case MoodIndicator.Normal:
                return 1.0f;
            case MoodIndicator.Angry:
                return 0.75f;
        }
        return 0.0f;
    }
    public Dialogue_Starter_Character GetCharacterInterraction()
    {
        return _CharacterInterraction;
    }
    public void Update()
    {
        if(!_CharacterInterraction.GetDrageState()) MoveTo();
    }
    public void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
        Debug.Log("Character new position: "+ targetPosition);
    }
    public bool HasArrived()
    {
        Vector2 PositionVector2 = transform.position;
        Debug.Log(PositionVector2 == targetPosition);
        return PositionVector2 == targetPosition;
    }
    public Vector2 GetTargetPosition()
    {
        return targetPosition;
    }
    public CharacterSO GetCharacterSO()
    {
        return characterSO;
    }
}
