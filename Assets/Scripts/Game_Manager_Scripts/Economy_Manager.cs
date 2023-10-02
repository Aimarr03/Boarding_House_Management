using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy_Manager : MonoBehaviour
{
    public static Economy_Manager _instance;
    public static event System.Action TransactionSuccesful;
    public static event System.Action TransactionFailed;
    public static event System.Action GainMoney;
    private int currency;

    public void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    public void UseMoney(int price)
    {
        if (price >= currency)
        {
            currency -= price;
            Debug.Log("Transaction succesful, currency: " + currency);
            TransactionSuccesful?.Invoke();
        }
        else
        {
            Debug.Log("Transaction failed, need more money!");
            TransactionFailed?.Invoke();
        }
    }
    public void GainRevenue(int revenue)
    {
        currency += revenue;
        GainMoney?.Invoke();
    }
    
}
