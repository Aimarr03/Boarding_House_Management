using System;
using UnityEngine;

[System.Serializable]
public class CustomGrid<T>{
    private int width;
    private int height;
    private T[,] grid;
    private float cellWidth;
    private float cellHeight;
    private Vector3 originPosition;

    public CustomGrid()
    {

    }
    public CustomGrid(int width, int height, float cellWidth,float cellHeight, Vector3 originPosition, Func<T> CreateInstance)
    {
        this.width = width;
        this.height = height;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.originPosition = originPosition;

        grid = new T[width, height];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x,y] = CreateInstance();
            }
        }
        this.originPosition = originPosition;
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        float xOffset = x * cellWidth;
        float yOffset = y * cellHeight;
        return new Vector3(xOffset,yOffset) +originPosition;
    }
    public Vector3 GetMiddleWorldPosition(int x, int y)
    {
        float xOffset = (0.5f + x) * cellWidth;
        float yOffset = (0.5f + y) * cellHeight;
        return new Vector3(xOffset, yOffset, 0) + originPosition;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x/cellWidth);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellHeight);
    }
    public void SetValue(int x,int y, T value)
    {
        if(x>=0 && y>=0 && x<width && y < height)
        {
            grid[x,y] = value;
            //textMesh[x,y].text = grid[x,y].ToString();
        }
    }
    public bool Buildable(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
    public void SetValue(Vector3 worldPosition, T value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return grid[x,y];
        }
        else
        {
            return default(T);
        }
    }
    public T GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    public void GetSize(out int width, out int height)
    {
        width = this.width; 
        height = this.height;
    }
}

