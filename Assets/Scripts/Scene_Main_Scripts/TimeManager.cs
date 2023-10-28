using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    [SerializeField] TextMeshProUGUI monthDisplay;
    [SerializeField] TextMeshProUGUI dateDisplay;



    [SerializeField] private float maxTime;
    public event Action ChangeSection;
    public event Action ChangeDate;
    
    private float currentTime;
    private int month;
    private int date;

    private float currentSpeed;
    private float normalSpeed;
    private float doubleSpeed;
    public bool pauseTime;

    private int triggerAddLine;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) return; 
        instance = this;
        month = 1;
        date = 1;
        normalSpeed = 1f;
        doubleSpeed = 2f;
        currentSpeed = normalSpeed;
        currentTime = maxTime;
        triggerAddLine = 0;
        DisplayDate();
    }

    // Update is called once per frame
    void Update()
    {
        DayCountDown();
    }
    private void NextDay()
    {
        date++;
        triggerAddLine++;
        if(date == 30)
        {
            month++;
            date = 1;
        }
        if(date % 7 == 0)
        {
            ChangeSection?.Invoke();
        }
        DisplayDate();
        if (CheckProbability(date % 10))
        {
            RandomEventOccur();
        }
        if(CleanManager.instance.furnitures.Count > 0)
        {
            CleanManager.instance.GainDirtyFurniture(CleanManager.instance.furnitures.Count);
        }
        EconomyManager.instance.GainRevenue();
        EconomyManager.instance.PaymentRoom();
        if (!EconomyManager.instance.CheckRoomEmpty())
        {
            triggerAddLine = 0;
        }
        if(triggerAddLine % QueueManager.instance.defaultDuration == 0)
        {
            triggerAddLine = 0;
            QueueManager.instance.AddNewLine();
        }

        ReputationManager.instance.ResetReputation();
        EconomyManager.instance.RevenueStream();
        CleanManager.instance.RevenueStream();

        ChangeDate?.Invoke();
    }
    private void DayCountDown()
    {
        if (!pauseTime)
        {
            currentTime -= Time.deltaTime * currentSpeed;
            if(currentTime <= 0)
            {
                NextDay();
                currentTime = maxTime;
            }
        }
    }
    private bool CheckProbability(int input)
    {
        float baseProbability = 0.1f;
        if(input != 0)
        {
            baseProbability = baseProbability * input;
            return UnityEngine.Random.value < baseProbability;
        }
        else
        {
            return true;
        }
    }
    public void RandomEventOccur()
    {
        EconomyManager.instance.SetBrokenRoom();
    }
    private void DisplayDate()
    {
        monthDisplay.text = "Month: " + month.ToString();
        dateDisplay.text = "Date: " + date.ToString();
    }
    public void Pause()
    {
        pauseTime = true;
    }
    public void NormalSpeed()
    {
        pauseTime = false;
        currentSpeed = normalSpeed;
    }
    public void FasterSpeed()
    {
        pauseTime = false;
        currentSpeed = doubleSpeed;
    }
}
