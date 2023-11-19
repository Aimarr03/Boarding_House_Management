using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleaningUI : MonoBehaviour
{
    [SerializeField] private Cleaning cleaningObject;
    [SerializeField] private Image progressUI;
    public void Awake()
    {
        gameObject.SetActive(false);
        cleaningObject.progressOccur += CleaningObject_progressOccur;
        cleaningObject.HoldingOccured += CleaningObject_HoldingOccured;
    }

    private void CleaningObject_HoldingOccured(bool status)
    {
        gameObject.SetActive(status);
    }

    private void CleaningObject_progressOccur(float progressNormalized)
    {
        if(progressNormalized == 0 ||  progressNormalized == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            progressUI.fillAmount = progressNormalized;
        }
    }
}
