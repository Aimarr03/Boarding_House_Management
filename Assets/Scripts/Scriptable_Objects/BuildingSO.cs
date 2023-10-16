using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Create new Building SO", fileName ="Building 01")]
public class BuildingSO : ScriptableObject
{
    [Header("Building Characteristics")]
    public string roomName;
    public Transform prefab;
    public List<Sprite> roomType;
    public int height;
    public int width;
    [Header("Economy Characteristics")]
    public int costPurchase;
    public int baseRevenue;
    public int baseTax;

    public List<Vector2Int> GetObjectSize(Vector2Int offset)
    {
        List<Vector2Int> ObjectSize = new List<Vector2Int>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                ObjectSize.Add(offset + new Vector2Int(x, y));
            }
        }
        return ObjectSize;
    }
}
