using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Transform UpgradeUI;
    [SerializeField] private FurnitureListSO FurnitureList;

    [SerializeField] private Transform FormatUI;
    [SerializeField] private Transform UpgradeContainer;

    public void Awake()
    {
        SetDisplay(true);
        foreach (FurnitureSO furniture in FurnitureList.FurnitureList)
        {
            Transform FurnitureUI = Instantiate(FormatUI, UpgradeContainer);
            FurnitureBuildingUI currentRoomBuilding = FurnitureUI.GetComponent<FurnitureBuildingUI>();
            currentRoomBuilding.SetFurnitureSO(furniture);
            FurnitureUI.gameObject.SetActive(true);
        }
    }
    public void SetDisplay(bool input)
    {
        UpgradeUI.gameObject.SetActive(input);
    }
}
