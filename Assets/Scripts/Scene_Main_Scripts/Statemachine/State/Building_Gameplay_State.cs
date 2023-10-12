using System;
using System.Collections.Generic;
using UnityEngine;

public class Building_Gameplay_State : Gameplay_StateDefault
{
    private BuildingSO _buildingSO;
    private InterractableObject _currentInterractedObject;
    public static event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
    private CustomGrid<GridObject> _grid;
    private bool interacting;
    public Building_Gameplay_State(GameplayScene scene, Gameplay_StateMachine stateMachine) : base(scene, stateMachine)
    {
        _grid = new CustomGrid<GridObject>(9, 12, 3, new Vector3(-20, -30, 0), () => new GridObject());
        _buildingSO = _scene._buildingSO;
    }
    public override void EnteringState()
    {
        Debug.Log("Current State is Building!");
        base.EnteringState();
    }

    public override void Hovering()
    {
        base.Hovering();
    }

    public override void OnClicked()
    {
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);

            List<Vector2Int> ObjectSize = _buildingSO.GetObjectSize(new Vector2Int(x, y));
            bool canBuild = true;
            foreach (Vector2Int currentGrid in ObjectSize)
            {
                /*
                GridObject currentGridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                if (currentGridObject == null) break;
                if (!_grid.GetValue(currentGrid.x, currentGrid.y).IsBuildable())
                {
                    canBuild = false; 
                    break;
                }*/
            }

            if (_grid.Buildable(x, y) && canBuild)
            {
                Transform buildingInstantiated = _scene.CustomInstantiate(_buildingSO.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                /*
                foreach(Vector2Int currentGrid in  ObjectSize)
                {
                    GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                    if (gridObject == null) break;
                    gridObject.SetBuilding(buildingInstantiated);
                } */
                _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
            }
            if (_currentInterractedObject != null)
            {
                interacting = true;
                _currentInterractedObject.Interracted();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(MousePositionVector3));
        }
    }


    public override void ExitingState()
    {
        base.ExitingState();
        Debug.Log("Building State exit!");
    }
}
