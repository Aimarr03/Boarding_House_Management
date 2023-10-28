using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour
{
    public static CleanManager instance;
    public List<Furniture> furnitures;
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        furnitures = new List<Furniture>();
    }
    public void GainDirtyFurniture(int currentIterate)
    {
        if(currentIterate <=0) 
            return;
        int index = Random.Range(0, furnitures.Count);
        Cleaning currentCleaning = furnitures[index].GetCleaning();
        if (currentCleaning.GetInterractState())
        {
            GainDirtyFurniture(currentIterate-1);
        }
        else
        {
            currentCleaning.SetDoingCleaning(false);
        }
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
