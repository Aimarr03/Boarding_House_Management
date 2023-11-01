using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaning : InterractableObject, IHasProgress
{
    private bool DoneCleaning = false;
    public float _durationCleaning;
    public Transform cleaningPosition;
    private float currentTimeCleaning;

    [SerializeField] private Transform DirtyIndicator;

    public Color defaultColor;
    public Color dirtyColor;
    public event Action<float> progressOccur;
    public event Action<bool> HoldingOccured;
    private bool isHolding;
    public void Start()
    {
        isHolding = false;
        DirtyIndicator.gameObject.SetActive(true);
    }
    public override void HoldInterraction()
    {
        base.HoldInterraction();
        Debug.Log("IsBusy " + ManagerCharacter.instance.IsBusy());
        if(DoneCleaning)
        {
            return;
        }
        ManagerCharacter.instance.DoAction(this);
        isHolding = true;
        currentTimeCleaning += Time.deltaTime;
        progressOccur?.Invoke(currentTimeCleaning/_durationCleaning);
        HoldingOccured?.Invoke(isHolding);
        if(currentTimeCleaning >= _durationCleaning && !DoneCleaning)
        {
            isHolding = false;
            DirtyIndicator.gameObject.SetActive(false);
            HoldingOccured?.Invoke(isHolding);
            DoneCleaning = true;
            Debug.Log("Done Cleaning");
        }
    }
    public void SetDirtyIndicator(bool input)
    {
        DirtyIndicator.gameObject.SetActive(!input);
    }
    public void ResetDuration()
    {
        ManagerCharacter.instance.StopAction();
        currentTimeCleaning = 0;
        isHolding = false;
        HoldingOccured?.Invoke(isHolding);
        Debug.Log("Reset");
    }
    public void SetDoingCleaning(bool input)
    {
        DoneCleaning = input;
        DirtyIndicator.gameObject.SetActive(!input);
    }
    public bool GetCleanStatus()
    {
        return DoneCleaning;
    }
}
