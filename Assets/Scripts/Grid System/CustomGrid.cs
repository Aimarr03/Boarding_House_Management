using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CustomGrid {
    private int width;
    private int height;
    private int[,] grid;
    private float cellSize;
    private TextMesh[,] textMesh;
    private Vector3 originPosition;

    public CustomGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        grid = new int[width, height];
        textMesh = new TextMesh[width, height];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                textMesh[x, y] = UtilsClass.CreateWorldText(grid[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 5, Color.white);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine((GetWorldPosition(0, height)), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine((GetWorldPosition(width, 0)), GetWorldPosition(width, height), Color.white, 100f);
        this.originPosition = originPosition;
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize +originPosition;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x-originPosition.x)/cellSize);
        y = Mathf.FloorToInt((worldPosition.y-originPosition.y) / cellSize);
    }
    public void SetValue(int x,int y, int value)
    {
        if(x>=0 && y>=0 && x<width && y < height)
        {
            grid[x,y] = value;
            textMesh[x,y].text = grid[x,y].ToString();
        }
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return grid[x,y];
        }
        else
        {
            return 0;
        }
    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}