using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] Transform BuildingUI;
    [SerializeField] private BuildingListSO buildingListSO;

    [SerializeField] private Transform RoomBuildingUIFormat;
    [SerializeField] private Transform RoomBuildingContainer;

    private void Awake()
    {
        SetDisplay(true);
        foreach(BuildingSO building in buildingListSO.buildings)
        {
            Transform RoomUI = Instantiate(RoomBuildingUIFormat, RoomBuildingContainer);
            RoomBuildingUI currentRoomBuilding = RoomUI.GetComponent<RoomBuildingUI>();
            currentRoomBuilding.SetBuildingSO(building);
            RoomUI.gameObject.SetActive(true);
        }
    }
    public void SetDisplay(bool input)
    {
        BuildingUI.gameObject.SetActive(input);
    }
}
