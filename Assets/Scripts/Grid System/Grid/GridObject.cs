using UnityEngine;

public class GridObject
{
    private Transform building;
    private Transform visualBuilding;
    public void SetVisualBuilding(Transform building)
    {
        this.visualBuilding = building;
    }
    public void SetVisual(bool input)
    {
        visualBuilding.gameObject.SetActive(input);
    }
    public void SetBuilding(Transform building)
    {
        this.building = building;
    }
    public Transform GetBuilding()
    {
        return building;
    }
    public void ClearBuilding()
    {
        building = null;
    }
    public bool IsBuildable()
    {
        //Debug.Log(building == null);
        return building == null;
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
