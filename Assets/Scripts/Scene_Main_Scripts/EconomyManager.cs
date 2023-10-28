using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public Dictionary<string, Room> roomObtain;
    public List<Room> roomObtainList;
    [SerializeField] private TextMeshProUGUI coinUI;
    public static EconomyManager instance;
    private int Currency;

    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        Currency = 1000;
        UpdateCoinDisplay();
        roomObtain = new Dictionary<string, Room>();
        roomObtainList = new List<Room>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool CheckMoney(int price)
    {
        return Currency >= price;
    }

    public void UseMoney(int price)
    {
        if(CheckMoney(price))
        {
            Currency -= price;
            UpdateCoinDisplay();
        }
    }
    public void SetBrokenRoom()
    {
        if (roomObtainList.Count > 0)
        {
            int index = Random.Range(0, roomObtainList.Count - 1);
            roomObtainList[index].GetBrokenIndicator().SetBrokenState(true);
            Debug.Log(roomObtainList[index].GetRoomIndex() + " is broken!");
        }
    }
    public void FixBrokenRoom(Room room, int cost)
    {
        UseMoney(cost);
        foreach(Room currentRoom in roomObtainList)
        {
            if (currentRoom.GetRoomIndex() == room.GetRoomIndex())
            {
                currentRoom.GetBrokenIndicator().SetBrokenState(false);
                Debug.Log(currentRoom.GetRoomIndex() + " is fixed");
                break;
            }
        }
    }
    public void UpdateCoinDisplay()
    {
        coinUI.text = Currency.ToString();
    }
    public void GainRevenue()
    {
        float revenue = 0;
        foreach(Room room in roomObtain.Values)
        {
            if (room.GetBrokenIndicator().GetBrokenState() || room.getRoomSlot().isEmpty()) continue;
            Character currentCharacter = room.getRoomSlot().GetCharacter();
            float multiplyRevenue = currentCharacter.GetMultiplyMoodIndicator();
            revenue += room.GetProfit() * multiplyRevenue;
        }
        Currency += (int)revenue;
        UpdateCoinDisplay();
    }
    public void PaymentRoom()
    {
        float cost = 0;
        foreach(Room room in roomObtain.Values)
        {
            cost += room.GetCost();
        }
        Currency -= (int)cost;
        UpdateCoinDisplay();
    }
    public void GainRevenue(int revenue)
    {
        Currency += revenue;
        UpdateCoinDisplay();
    }
    public void RevenueStream()
    {
        if (!(roomObtainList.Count > 0)) return;
        foreach(Room currentRoom in roomObtainList)
        {
            BrokenIndicator currentBrokenIndicator = currentRoom.GetBrokenIndicator();
            RoomSlot currentRoomSlot = currentRoom.getRoomSlot();
            BuildingSO currentBuildingSO = currentRoom.GetBuildingSO();
            int currentReputationPoint = currentBuildingSO.reputationPoint;
            if (currentBrokenIndicator.GetBrokenState())
            {
                ReputationManager.instance.ReduceReputation(currentReputationPoint);
            }
            else
            {
                if (!currentRoomSlot.isEmpty())
                {
                    currentReputationPoint = (int)(currentReputationPoint * 1.5f);
                }
                ReputationManager.instance.GainReputation(currentReputationPoint);
            }
        }
    }
    public bool CheckRoomEmpty()
    {
        if (!(roomObtainList.Count > 0)) return false;
        foreach(Room currentRoom in roomObtainList)
        {
            if (!currentRoom.getRoomSlot().isEmpty())
            {
                return false;
            }
        }
        return true;   
    }
}
