using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create new Building SO", fileName ="Building 01")]
public class BuildingSO : ScriptableObject
{
    public Transform prefab;
    [Header("Building Size")]
    public int height;
    public int width;
}
