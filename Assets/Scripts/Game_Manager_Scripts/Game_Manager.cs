using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private float _TimeOfDay;
    private float _CurrentTime;

    private float _currentSped;
    private float _normalSpeed;
    private float _fastForward;
    void Start()
    {
        _TimeOfDay = 30f;
        _CurrentTime = 0;
        _normalSpeed = 1f;
        _fastForward = 2f;
        _currentSped = _normalSpeed;

        Economy_Manager.GainMoney += Economy_Manager_GainMoney;
    }

    private void Economy_Manager_GainMoney()
    {
        Debug.Log("Gain Money!!!");
    }

    // Update is called once per frame
    void Update()
    {
        StopWatch();
    }
    private void StopWatch()
    {
        if (_CurrentTime <= _TimeOfDay)
        {
            _CurrentTime += Time.deltaTime * _currentSped;
            Debug.Log("Current Time: " + _CurrentTime);
        }
        else
        {
            Debug.Log("It is the next day!");
            Economy_Manager._instance.GainRevenue(30);
            
            _CurrentTime = 0;
        }
    }
}
