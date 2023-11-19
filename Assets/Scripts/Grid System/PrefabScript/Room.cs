using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private BuildingSO buildingSO;
    [SerializeField] private BrokenIndicator brokenIndicator;
    [SerializeField] private RoomSlot roomSlot;

    private string room_Index;
    
    public BuildingSO GetBuildingSO()
    {
        return buildingSO;
    }
    public int GetProfit()
    {
        return buildingSO.baseRevenue;
    }
    public int GetCost()
    {
        return buildingSO.baseTax;
    }
    public void SetRoomIndex(int index)
    {
        room_Index = "room_" + index;
    }
    public void SetRoomIndex(string input)
    {
        room_Index = input;
    }
    public string GetRoomIndex()
    {
        return room_Index;
    }
    public BrokenIndicator GetBrokenIndicator()
    {
        return brokenIndicator;
    }
    public RoomSlot getRoomSlot()
    {
        return roomSlot;
    }
}
