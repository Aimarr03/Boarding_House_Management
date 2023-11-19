using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Guest Room", menuName ="Create Guest Room SO")]
public class GuestRoomSO : BuildingSO
{
    public List<FurnitureSO> Furnitures;
    public FurnitureSO GetFurnitureSO(int index)
    {
        return Furnitures[index];
    }
}
