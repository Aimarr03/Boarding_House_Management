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
