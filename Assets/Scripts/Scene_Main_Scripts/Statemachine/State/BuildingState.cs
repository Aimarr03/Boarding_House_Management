using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : StateDefault
{
    [SerializeField] public BuildingSO _buildingSO;
    [SerializeField] private LayerMask interractedLayer;
    private CustomGrid<GridObject> _grid;
    public void Start()
    {
        _grid = new CustomGrid<GridObject>(3, 5, 9, 3, new Vector3(-15, -5, 0), () => new GridObject());
    }
    public override void EnterState()
    {
        base.EnterState();
        Enabled = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        Enabled = false;
    }
    public void Update()
    {
        if (Enabled)
        {
            OnClick();
        }
    }

    public override void OnClick()
    {
        base.OnClick();
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);

            List<Vector2Int> ObjectSize = _buildingSO.GetObjectSize(new Vector2Int(x, y));
            bool canBuild = true;
            //Check whether a grid is occupied or not
            foreach (Vector2Int currentGrid in ObjectSize)
            {

                GridObject currentGridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                if (currentGridObject == null) break;
                if (!_grid.GetValue(currentGrid.x, currentGrid.y).IsBuildable())
                {
                    canBuild = false;
                    break;

                }
            }
            if (_grid.Buildable(x, y) && canBuild)
            {
                Transform buildingInstantiated = Instantiate(_buildingSO.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                foreach (Vector2Int currentGrid in ObjectSize)
                {
                    GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                    if (gridObject == null) break;
                    gridObject.SetBuilding(buildingInstantiated);
                }
                _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(MousePositionVector3));
        }
    }

    public override void Hovering()
    {
        base.Hovering();
    }
}
