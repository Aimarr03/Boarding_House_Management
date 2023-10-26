using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] Transform BuildingUI;
    [SerializeField] private BuildingListSO buildingListSO;
    [SerializeField] private List<BuildingSO> currentListSO;
    [SerializeField] private Transform RoomBuildingUIFormat;
    [SerializeField] private Transform RoomBuildingContainer;

    private int gameSectionIndex;
    private void Awake()
    {
        DialogueState.instance.BoughtMoney += Instance_BoughtMoney;
        gameSectionIndex = 0;
        SetDisplay(true);
    }

    private void Start()
    {
        UnlockRoom();
    }
    private void Instance_BoughtMoney()
    {
        UnlockRoom();
    }


    private void UnlockRoom()
    {
        if(gameSectionIndex < buildingListSO.buildings.Count)
        {

            currentListSO.Add(buildingListSO.buildings[gameSectionIndex]);
            foreach (Transform buildingUI in RoomBuildingContainer)
            {
                if (buildingUI != RoomBuildingUIFormat)
                {
                    Destroy(buildingUI.gameObject);
                }
            }
            foreach (BuildingSO building in currentListSO)
            {
                Transform RoomUI = Instantiate(RoomBuildingUIFormat, RoomBuildingContainer);
                RoomBuildingUI currentRoomBuilding = RoomUI.GetComponent<RoomBuildingUI>();
                currentRoomBuilding.SetBuildingSO(building);
                RoomUI.gameObject.SetActive(true);
            }
            gameSectionIndex++;
        }
    }
    public void SetDisplay(bool input)
    {
        BuildingUI.gameObject.SetActive(input);
    }
}
