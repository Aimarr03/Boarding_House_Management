using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    [SerializeField] private FurnitureSO furnitureSO;
    public FurnitureSO GetFurnitureSO()
    {
        return furnitureSO;
    }
    public void SetFurnitureSO(FurnitureSO input)
    {
        furnitureSO = input;
    }
}
