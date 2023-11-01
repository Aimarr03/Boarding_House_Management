using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestRoomSlot : RoomSlot
{
    private int waitingDay;

    public override void Awake()
    {
        base.Awake();
        waitingDay = 0;
    }
    protected override void Instance_ChangeDate()
    {
        base.Instance_ChangeDate();
        if(!isEmpty() && room is GuestRoom)
        {
            waitingDay += 1;
            if(waitingDay > 1)
            {
                character.ChangeMood(Character.MoodIndicator.Dissapointed);
                SetRoom(null);
                waitingDay = 0;
                ReputationManager.instance.ModifiyDefaultReputation(-5);
            }
        }
        else if(isEmpty())
        {
            waitingDay = 0;
        }
    }
  
}
