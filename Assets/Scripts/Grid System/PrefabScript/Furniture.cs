using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    [SerializeField] private FurnitureSO furnitureSO;
    [SerializeField] private Cleaning cleaning;
    public FurnitureSO GetFurnitureSO()
    {
        return furnitureSO;
    }
    public void SetFurnitureSO(FurnitureSO input)
    {
        furnitureSO = input;
    }
    public Cleaning GetCleaning()
    {
        return cleaning;
    }
}
