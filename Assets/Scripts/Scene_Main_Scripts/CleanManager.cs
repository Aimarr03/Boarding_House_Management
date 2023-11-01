using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour, IDataPersistance
{
    public static CleanManager instance;
    public List<Furniture> furnitures;
    public List<FurnitureSO> furnitureSOs;
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        furnitures = new List<Furniture>();
    }
    public void GainDirtyFurniture()
    {
        if (furnitures.Count <= 0) return;
        int index = Random.Range(0, furnitures.Count-1);
        Cleaning currentCleaning = furnitures[index].GetCleaning();
        currentCleaning.SetDoingCleaning(false);
    }
    public void SaveScene(ref GameData gameData)
    {
        gameData.furnituresData.Clear();
        foreach(Furniture furniture in furnitures)
        {
            GameData.FurnitureData furnitureData = new GameData.FurnitureData();
            FurnitureSO furnitureSO = furniture.GetFurnitureSO();
            furnitureData.furnitureName = furnitureSO.furnitureName;
            furnitureData.hasCleaned = furniture.GetCleaning().GetCleanStatus();
            gameData.furnituresData.Add(furnitureData);
        }
    }

    public void LoadScene(GameData gameData)
    {

        // Load the new data.
        List<GameData.FurnitureData> furnitureData = gameData.furnituresData;
        foreach (GameData.FurnitureData currentFurnitureData in furnitureData)
        {
            FurnitureSO furnitureSO = GetFurnitureSO(currentFurnitureData.furnitureName);
            BuildingState.instance.guestRoom.AddedFurniture(furnitureSO);
            Furniture furniture = BuildingState.instance.guestRoom.GetFurniture(furnitureSO);
            furniture.GetCleaning().SetDirtyIndicator(currentFurnitureData.hasCleaned);
        }
    }

    public FurnitureSO GetFurnitureSO(string furnitureName)
    {
        foreach (FurnitureSO furnitureSO in furnitureSOs)
        {
            if (furnitureSO.furnitureName == furnitureName)
            {
                return furnitureSO;
            }
        }
        return null;
    }
    private void ClearOldData()
    {
        foreach (var furniture in furnitures)
        {
            Destroy(furniture.gameObject);
        }
        furnitures.Clear();
    }


    public void RevenueStream()
    {
        if (!(furnitures.Count > 0)) return;
        foreach(Furniture currentFurniture in furnitures)
        {
            Cleaning currentCleaning = currentFurniture.GetCleaning();
            FurnitureSO currentFurnitureSO = currentFurniture.GetFurnitureSO();
            if (currentCleaning.GetCleanStatus())
            {
                ReputationManager.instance.GainReputation(currentFurnitureSO.reputationPoint);
            }
            else
            {
                ReputationManager.instance.ReduceReputation(currentFurnitureSO.reputationPoint);
            }
        }
    }

}
