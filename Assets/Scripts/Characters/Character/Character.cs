using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterSO characterSO;
    [SerializeField] private Dialogue_Starter_Character _CharacterInterraction;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float _movementSpeed;
    [SerializeField] CharacterMoodIndicator moodIndicator;

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
        targetPosition = transform.position;
    }
    public void Start()
    {
        ChangeMood(MoodIndicator.Normal);
    }
    public void MoveTo()
    {
        Vector2 PositionVector2 = transform.position;
        if(PositionVector2 != targetPosition)
        {
            transform.position = Vector2.MoveTowards(PositionVector2, targetPosition, _movementSpeed);
            _CharacterInterraction.setInterract(false);
        }
        else
        {
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
    }
}
