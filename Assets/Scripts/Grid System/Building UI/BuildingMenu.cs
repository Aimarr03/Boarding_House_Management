using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] Transform BuildingUI;
    [SerializeField] private BuildingListSO buildingListSO;
    [SerializeField] private List<BuildingSO> currentListSO;
    [SerializeField] private Transform RoomBuildingUIFormat;
    [SerializeField] private Transform RoomBuildingContainer;

    public int gameSectionIndex;
    public Color disabledColor;
    private void Awake()
    {
        gameSectionIndex = 1;
        SetDisplay(true);
        
    }
    public void OnDestroy()
    {
        DialogueState.instance.BoughtMoney -= Instance_BoughtMoney;
    }

    private void Start()
    {
        for(int i = 0; i <= gameSectionIndex; i++)
        {
            AltUnlockRoom(i);
        }
        DialogueState.instance.BoughtMoney += Instance_BoughtMoney;
    }
    private void Instance_BoughtMoney()
    {
        UnlockRoom();
    }


    public void UnlockRoom()
    {
        if(gameSectionIndex < buildingListSO.buildings.Count-1)
        {
            gameSectionIndex++; 
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
            Debug.Log("Index: "+gameSectionIndex);
        }
    }
    public void AltUnlockRoom(int iteration)
    {
        if (gameSectionIndex < buildingListSO.buildings.Count)
        {
            Debug.Log("Added Furniture UI");
            currentListSO.Add(buildingListSO.buildings[iteration]);
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
            Debug.Log("Index: " + gameSectionIndex);
        }
    }
    public void SetDisplay(bool input)
    {
        BuildingUI.gameObject.SetActive(input);
    }
}
