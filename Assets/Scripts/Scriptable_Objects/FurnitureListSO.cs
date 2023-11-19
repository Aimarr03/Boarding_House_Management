using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Furniture List 0", menuName ="Create new Furniture List SO")]
public class FurnitureListSO : ScriptableObject
{
    public List<FurnitureSO> FurnitureList;
}
