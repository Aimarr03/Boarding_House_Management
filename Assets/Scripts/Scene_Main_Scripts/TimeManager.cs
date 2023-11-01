using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistance
{
    public static TimeManager instance;
    [SerializeField] TextMeshProUGUI monthDisplay;
    [SerializeField] TextMeshProUGUI dateDisplay;

    public event Action<TimeState> TimeChanged;

    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer02;
    public Color morningColor;
    public Color nightColor;

    [SerializeField] private float maxTime;
    public event Action ChangeSection;
    public event Action ChangeDate;
    
    private float currentTime;
    private int month;
    private int date;
    private int probability;

    private float currentSpeed;
    private float normalSpeed;
    private float doubleSpeed;

    private int NegativeMoneyDuration;
    public bool pauseTime;
    public TimeState currentTimeState;
    public enum TimeState
    {
        pause,
        normal,
        fast
    }

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
        NegativeMoneyDuration = 0;
        DisplayDate();
    }
    void Start()
    {
        StartCoroutine(ChangeBackgroundColor());
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameOverStatus || GameManager.instance.GameIsPaused) return;
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
        if(date % 14 == 0)
        {
            ChangeSection?.Invoke();
        }
        DisplayDate();
        if (CheckProbability(date % 14))
        {
            RandomEventOccur();
        }
        if(CheckProbability(date % 16))
        {
            AltRandomEventOccur();
        }
        if (!EconomyManager.instance.CheckMoney(0))
        {
            NegativeMoneyDuration++;
            if(NegativeMoneyDuration >= 2)
            {
                GameManager.instance.SetGameStatus(GameManager.TypeOfGameStatus.NoMoney);
            }
        }
        else
        {
            NegativeMoneyDuration = 0;
        }
        EconomyManager.instance.GainRevenue();
        EconomyManager.instance.PaymentRoom();

        if(triggerAddLine % QueueManager.instance.defaultDuration == 0 || !(EconomyManager.instance.CheckRoomEmpty()))
        {
            triggerAddLine = 0;
            QueueManager.instance.AddNewLine();
        }
        ReputationManager.instance.ResetReputation();
        EconomyManager.instance.RevenueStream();
        CleanManager.instance.RevenueStream();
        QueueManager.instance.CheckWaitingDay();
        QueueManager.instance.CheckAngryCustomer();

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
        float baseProbability = 0.6f;
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
    public void AltRandomEventOccur()
    {
        CleanManager.instance.GainDirtyFurniture();
    }
    private void DisplayDate()
    {
        monthDisplay.text = "Month: " + month.ToString();
        dateDisplay.text = "Date: " + date.ToString();
    }
    public void Pause()
    {
        pauseTime = true;
        TimeChanged?.Invoke(TimeState.pause);
        currentTimeState = TimeState.pause;
    }
    public void NormalSpeed()
    {
        pauseTime = false;
        currentSpeed = normalSpeed;
        TimeChanged?.Invoke(TimeState.normal);
        currentTimeState = TimeState.normal;
    }
    public void FasterSpeed()
    {
        pauseTime = false;
        currentSpeed = doubleSpeed;
        TimeChanged?.Invoke(TimeState.fast);
        currentTimeState = TimeState.fast;
    }

    public void LoadScene(GameData gameData)
    {
        date = gameData.date;
        month = gameData.month;
        DisplayDate();
    }

    public void SaveScene(ref GameData gameData)
    {
        gameData.date = date;
        gameData.month = month;
    }

    private IEnumerator ChangeBackgroundColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(maxTime * 0.2f);
            yield return LerpColor(morningColor, nightColor, maxTime  * 0.2f );
            yield return new WaitForSeconds(maxTime * 0.3f);
            yield return LerpColor(nightColor, morningColor, maxTime  *  0.3f);
        }
    }

    private IEnumerator LerpColor(Color startColor, Color targetColor, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            backgroundSpriteRenderer02.color = Color.Lerp(startColor, targetColor, time / duration);
            backgroundSpriteRenderer.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }
        backgroundSpriteRenderer.color = targetColor;
    }
}
