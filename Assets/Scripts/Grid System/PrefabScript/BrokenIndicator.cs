using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenIndicator : InterractableObject
{
    [SerializeField] private Room room;
    [SerializeField] private Transform brokenIndicator;
    public Transform repairingPosition;
    private bool brokenState;

    private void Start()
    {
        SetBrokenState(false);
    }
    public bool GetBrokenState()
    {
        return brokenState;
    }
    public void SetBrokenState(bool value)
    {
        brokenState = value;
        brokenIndicator.gameObject.SetActive(value);
    }
    public override void Interracted()
    {
        if (ManagerCharacter.instance.IsBusy()) return;
        base.Interracted();
        Debug.Log("Broken Indicator Interracted");
        bool canFix = EconomyManager.instance.CheckMoney(10);
        if (canFix)
        {
            EconomyManager.instance.FixBrokenRoom(room, 10);
            ManagerCharacter.instance.DoAction(this);
        }
    }
}
