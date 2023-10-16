using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Building List", menuName ="Create new Building List")]
public class BuildingListSO : ScriptableObject
{
    public List<BuildingSO> buildings;
    public BuildingSO GetBuilding(int index)
    {
        return buildings[index];
    }
}
