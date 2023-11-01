using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSlot : MonoBehaviour
{
    [SerializeField] protected Transform roomSlot;
    [SerializeField] protected Character character;
    [SerializeField] protected Room room;

    //Trying to have indicator 
    private int occupyNormal;
    private int occupyAbnormal;

    public virtual void Awake()
    {
        occupyAbnormal = 0;
        occupyNormal = 0;
        TimeManager.instance.ChangeDate += Instance_ChangeDate;
    }
    public void OnDestroy()
    {
        TimeManager.instance.ChangeDate -= Instance_ChangeDate;
    }

    protected virtual void Instance_ChangeDate()
    {
        if (!isEmpty() && !(room is GuestRoom))
        {
            if (room.GetBrokenIndicator().GetBrokenState())
            {
                occupyNormal = 0;
                occupyAbnormal += 1;
                if(occupyAbnormal > 2)
                {
                    character.ChangeMood(Character.MoodIndicator.Angry);
                    Debug.Log("Character mood changed into Angry");
                }
                if(occupyAbnormal > 4)
                {
                    character.ChangeMood(Character.MoodIndicator.Dissapointed);
                    Debug.Log("Character mood changed into Dissapointed");
                    SetRoom(null);
                    ReputationManager.instance.ModifiyDefaultReputation(-10);
                }
            }
            else
            {
                occupyAbnormal = 0;
                occupyNormal += 1;
                if(occupyNormal == 0 && character.GetCurrentMood() != Character.MoodIndicator.Happy)
                {
                    character.ChangeMood(Character.MoodIndicator.Normal);
                    Debug.Log("Character mood changed into Normal");
                }
                if(occupyNormal > 3)
                {
                    character.ChangeMood(Character.MoodIndicator.Happy);
                }
            }
        }
    }

    public bool isEmpty()
    {
        return character == null;
    }
    public Character GetCharacter()
    {
        return character;
    }
    public void SetRoom(Character character)
    {
        if (character == null)
        {
            this.character = null;
            return;
        }
        if (!isEmpty()) return;
        character.gameObject.transform.position = roomSlot.position;
        character.SetTargetPosition(roomSlot.position);
        character.GetCharacterInterraction().UpdateOriginalPosition(roomSlot.position);
        character.SetRoomSlot(this);
        this.character = character;
        QueueManager.instance.RemoveFromLine(character);
        character.ChangeMood(Character.MoodIndicator.Normal);
    }
    public Transform GetTransformRoomSlot()
    {
        return roomSlot;
    }
}
