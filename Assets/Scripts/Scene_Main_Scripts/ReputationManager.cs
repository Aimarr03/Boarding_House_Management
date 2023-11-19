using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationManager : MonoBehaviour, IDataPersistance
{
    public static ReputationManager instance;
    public event Action<StatusReputation> IndicatorChange;


    [SerializeField] private TextMeshProUGUI reputationVisual;

    [SerializeField] private int reputation;
    [SerializeField] private int maxReputation;
    [SerializeField] private int HighReputation;

    public StatusReputation reputationType;

    public enum StatusReputation
    {
        low,
        normal,
        high
    }
    private int defaultReputation;
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        reputation = 100;
        HighReputation = 200;
        maxReputation = 300;
        defaultReputation = reputation;
        reputationType = StatusReputation.normal;
        UpdateVisual();
    }
    public int GetReputation()
    {
        return reputation;
    }
    public void GainReputation(int input)
    {
        reputation += input;
        if(reputation > maxReputation) reputation = maxReputation;
        TriggerEvent();
        UpdateVisual();
    }
    public void ReduceReputation(int input)
    {
        reputation -= input;
        if(reputation < 0) reputation = 0;
        TriggerEvent();
        UpdateVisual();
    }
    public void TriggerEvent()
    {
        if(reputation < 100)
        {
            IndicatorChange?.Invoke(StatusReputation.low);
            reputationType = StatusReputation.low;
        }
        if(reputation >= 100 && reputation < HighReputation)
        {
            IndicatorChange?.Invoke(StatusReputation.normal);
            reputationType = StatusReputation.normal;
        }
        if(reputation >= HighReputation)
        {
            IndicatorChange?.Invoke(StatusReputation.high);
            reputationType= StatusReputation.high;
        }
    }
    public void UpdateVisual()
    {
        reputationVisual.text = reputation.ToString();
    }
    public void ResetReputation()
    {
        reputation = defaultReputation;
    }
    public void ModifiyDefaultReputation(int input)
    {
        defaultReputation += input;
    }

    public void LoadScene(GameData gameData)
    {
        reputation = gameData.reputation;
        Debug.Log("reputation = " + reputation);
        UpdateVisual();
    }

    public void SaveScene(ref GameData gameData)
    {
        gameData.reputation = reputation;
        Debug.Log("reputation saved = " + gameData.reputation);
    }
}
