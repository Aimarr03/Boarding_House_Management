using UnityEngine;

public class GridObject
{
    private Transform building;
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
        return building == null;
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
