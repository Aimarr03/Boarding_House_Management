using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    [SerializeField] private BuildingSO _buildingSO;
    private InterractableObject _currentInterractedObject;
    public static event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
    private CustomGrid<GridObject> _grid;

    private void Start()
    {
        _grid = new CustomGrid<GridObject>(9, 12, 3, new Vector3(-20,-30,0), () => new GridObject());
    }
    // Update is called once per frame
    void Update()
    {
        Hovering();
        OnClicked();
    }
    public void Hovering()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero, 100f,interractedLayer);
        //Debug.Log(hitInfo.collider != null);
        
        //If it hits something
        if(hitInfo.collider != null)
        {
            if (hitInfo.transform.TryGetComponent<InterractableObject>(out InterractableObject _currentInterractable))
            {
                //If it not the same, assign it
                if (_currentInterractedObject != _currentInterractable)
                {
                    SetCurrentInterractableObject(_currentInterractable);
                }
            }
            else SetCurrentInterractableObject(null);
        }
        else SetCurrentInterractableObject(null);

    }
    void OnClicked()
    {
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);

            List<Vector2Int> ObjectSize = _buildingSO.GetObjectSize(new Vector2Int(x,y));
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
                Transform buildingInstantiated = Instantiate(_buildingSO.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                /*
                foreach(Vector2Int currentGrid in  ObjectSize)
                {
                    GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                    if (gridObject == null) break;
                    gridObject.SetBuilding(buildingInstantiated);
                } */   
                _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
            }
            Debug.Log(_currentInterractedObject != null);
            Debug.Log(_currentInterractedObject);
            if(_currentInterractedObject != null)
            {
                _currentInterractedObject.Interracted();
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(MousePositionVector3));
        }
    }
    public void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        _currentInterractedObject = _InterractableObject;
        OnHovering?.Invoke(_currentInterractedObject);
    }
}
