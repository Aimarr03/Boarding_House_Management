using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public Dictionary<string, Room> roomObtain;
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
    public void UpdateCoinDisplay()
    {
        coinUI.text = Currency.ToString();
    }
    public void GainRevenue()
    {
        float revenue = 0;
        foreach(Room room in roomObtain.Values)
        {
            revenue += room.GetProfit();
        }
        Currency += (int)revenue;
        UpdateCoinDisplay();
    }
    public void GainRevenue(int revenue)
    {
        Currency += revenue;
        UpdateCoinDisplay();
    }

}
