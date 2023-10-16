using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monthDisplay;
    [SerializeField] TextMeshProUGUI dateDisplay;

    [SerializeField] private float maxTime;
    private float currentTime;
    private int month;
    private int date;
    // Start is called before the first frame update
    void Awake()
    {
        month = 1;
        date = 1;
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        DayCountDown();
    }
    private void NextDay()
    {
        date++;
        if(date == 30)
        {
            month++;
            date = 1;
        }
        monthDisplay.text = "Month: " + month.ToString();
        dateDisplay.text = "Date: " + date.ToString();
        EconomyManager.instance.GainRevenue();
    }
    private void DayCountDown()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            NextDay();
            currentTime = maxTime;
        }
    }
}
