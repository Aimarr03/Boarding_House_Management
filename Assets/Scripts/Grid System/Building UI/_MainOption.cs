using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MainOption : MonoBehaviour
{
    [SerializeField] private List<Transform> BuildingOption;
    public void Awake()
    {
        foreach (Transform t in BuildingOption)
        {
            t.gameObject.SetActive(false);
        }
    }
    public void DisplayOption(int index)
    {
        for(int i = 0; i < BuildingOption.Count; i++)
        {
            BuildingOption[i].gameObject.SetActive(false);
            if(index == i ) BuildingOption[i].gameObject.SetActive(true);
        }
    }
}
