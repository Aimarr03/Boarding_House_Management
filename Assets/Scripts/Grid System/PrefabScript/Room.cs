using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private BuildingSO buildingSO;
    private string room_Index;
    public BuildingSO GetBuildingSO()
    {
        return buildingSO;
    }
    public int GetProfit()
    {
        return buildingSO.baseRevenue;
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
}
