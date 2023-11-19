using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestRoom : Room
{
    [SerializeField] private List<Furniture> furnitures;
    public Transform ManagerOriginalPosition;
    public void AddedFurniture(FurnitureSO furnitureSO)
    {
        foreach(Furniture furniture in furnitures)
        {
            if(furniture.GetFurnitureSO() == furnitureSO)
            {
                furniture.gameObject.SetActive(true);
                CleanManager.instance.furnitures.Add(furniture);
            }
        }
    }
    public Furniture GetFurniture(FurnitureSO furnitureSO)
    {
        foreach (Furniture furniture in furnitures)
        {
            if (furniture.GetFurnitureSO() == furnitureSO)
            {
                return furniture;
            }
        }
        return null;
    }
}
