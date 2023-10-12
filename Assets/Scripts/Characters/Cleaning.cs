using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaning : InterractableObject
{
    public static Action<float> DoingCleaning;
    private bool DoneCleaning = false;
    [SerializeField] private float _durationCleaning;
    private float currentTimeCleaning;

    public override void HoldInterraction()
    {
        base.HoldInterraction();
        currentTimeCleaning += Time.deltaTime;
        DoingCleaning?.Invoke(currentTimeCleaning/_durationCleaning);
        if(currentTimeCleaning >= _durationCleaning && !DoneCleaning)
        {
            DoneCleaning = true;
            Debug.Log("Done Cleaning");
        }
    }
    public void ResetDuration()
    {
        currentTimeCleaning = 0;
        Debug.Log("Reset");
    }
    public void SetDoingCleaning(bool input)
    {
        DoneCleaning = input;
    }
    
}
