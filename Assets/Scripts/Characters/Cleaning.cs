using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaning : InterractableObject, IHasProgress
{
    private bool DoneCleaning = false;
    [SerializeField] private float _durationCleaning;
    private float currentTimeCleaning;

    public event Action<float> progressOccur;
    public event Action<bool> HoldingOccured;
    private bool isHolding;
    public void Start()
    {
        isHolding = false;
    }
    public override void HoldInterraction()
    {
        base.HoldInterraction();
        if(DoneCleaning)
        {
            return;
        }
        isHolding = true;
        currentTimeCleaning += Time.deltaTime;
        progressOccur?.Invoke(currentTimeCleaning/_durationCleaning);
        HoldingOccured?.Invoke(isHolding);
        if(currentTimeCleaning >= _durationCleaning && !DoneCleaning)
        {
            isHolding = false;
            HoldingOccured?.Invoke(isHolding);
            DoneCleaning = true;
            Debug.Log("Done Cleaning");
        }
    }
    public void ResetDuration()
    {
        currentTimeCleaning = 0;
        isHolding = false;
        HoldingOccured?.Invoke(isHolding);
        Debug.Log("Reset");
    }
    public void SetDoingCleaning(bool input)
    {
        DoneCleaning = input;
    }
    public bool GetCleanStatus()
    {
        return DoneCleaning;
    }
}
