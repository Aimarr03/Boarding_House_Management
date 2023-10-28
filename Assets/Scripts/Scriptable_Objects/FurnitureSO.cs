using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Furniture", menuName ="Create New Furniture SO")]
public class FurnitureSO : ScriptableObject
{
    public string furnitureName;
    [Header("Characteristics")]
    public Transform prefab;
    public Sprite image;
    public int cost;
    public int reputationPoint;
}
