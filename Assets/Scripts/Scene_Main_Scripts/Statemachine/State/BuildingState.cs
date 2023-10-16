using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : StateDefault
{
    public static BuildingState instance;

    [SerializeField] private BuildingSO _buildingSO;
    [SerializeField] private LayerMask interractedLayer;
    private CustomGrid<GridObject> _grid;
    int room_index = 0;
    public void Awake()
    {
        if (instance != null) return; 
        instance = this;
    }
    public void Start()
    {
        _grid = new CustomGrid<GridObject>(3, 5, 6, 3, new Vector3(-15, -5, 0), () => new GridObject());
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
        if(_buildingSO == null)
        {
            return;
        }
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
                //Check if the grid is offset from the maximum or minimmun on the x or y axis
                if (currentGridObject == null) break;
                if (!_grid.GetValue(currentGrid.x, currentGrid.y).IsBuildable())
                {
                    canBuild = false;
                    break;

                }
            }
            Debug.Log(EconomyManager.instance.CheckMoney(_buildingSO.costPurchase));
            if (_grid.Buildable(x, y) && canBuild && EconomyManager.instance.CheckMoney(_buildingSO.costPurchase))
            {
                string roomIndexString = "room_" + room_index;
                EconomyManager.instance.UseMoney(_buildingSO.costPurchase);
                Transform buildingInstantiated = Instantiate(_buildingSO.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                buildingInstantiated.TryGetComponent<Room>(out Room currentRoom);
                currentRoom.SetRoomIndex(roomIndexString);
                EconomyManager.instance.roomObtain.Add(roomIndexString, currentRoom);
                buildingInstantiated.GetComponent<SpriteRenderer>().sprite = _buildingSO.roomType[x];
                foreach (Vector2Int currentGrid in ObjectSize)
                {
                    GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                    //Check if the grid is offset from the maximum or minimmun on the x or y axis
                    if (gridObject == null) break;
                    gridObject.SetBuilding(buildingInstantiated);
                }
                _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
                room_index++;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(MousePositionVector3));
        }
    }
    public void SetBuildingSO(BuildingSO buildingSO)
    {
        this._buildingSO = buildingSO;
    }
    public override void Hovering()
    {
        base.Hovering();
    }
}
