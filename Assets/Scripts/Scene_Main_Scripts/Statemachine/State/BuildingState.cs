using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingState : StateDefault
{
    public static BuildingState instance;

    [SerializeField] private BuildingSO building;
    [SerializeField] private GuestRoomSO _guestRoomSO;
    [SerializeField] private LayerMask interractedLayer;


    [SerializeField] private Transform BuildingDialogue;
    [SerializeField]private Room room;
    private CustomGrid<GridObject> _grid;


    private int room_index = 0;
    public void Awake()
    {
        if (instance != null) return; 
        BuildingDialogue.gameObject.SetActive(false);
        instance = this;
    }
    public void Start()
    {
        _grid = new CustomGrid<GridObject>(3, 5, 6, 3, new Vector3(-15, -5, 0), () => new GridObject());
        BuildRoom(0, 0, _guestRoomSO);
    }
    public override void EnterState()
    {
        base.EnterState();
        BuildingDialogue.gameObject.SetActive(true);
        Enabled = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        BuildingDialogue.gameObject.SetActive(false);
        Enabled = false;
    }
    public void Update()
    {
        if (Enabled)
        {
            //Hovering();
            OnClick();
        }
    }
    public override void OnClick()
    {
        if(building == null)
        {
            return;
        }
        base.OnClick();
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);
            if(CheckBelow(x,y)) BuildRoom(x, y, building);
            //CheckBelow(x, y, building);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Alternative Clicked!");
            _grid.GetXY(MousePositionVector3, out int x, out int y);
            if(!CheckAbove(x,y)) SellRoom(x, y); 
        }
    }
    public bool CheckBelow(int x, int y)
    {
        if (_grid.Buildable(x, y))
        {
            if (y > 0)
            {
                return !_grid.GetValue(x, y - 1).IsBuildable();
            }
        }
        return false;
    }
    public bool CheckAbove(int x, int y)
    {
        _grid.GetSize(out int width, out int height);
        if (_grid.Buildable(x, y))
        {
            if(y < height)
            {
                Debug.Log(!_grid.GetValue(x, y + 1).IsBuildable());
                return !_grid.GetValue(x, y + 1).IsBuildable();
            }
        }
        return false;
    }
    public void SellRoom(int x, int y)
    {
        room = _grid.GetValue(x, y).GetBuilding().GetComponent<Room>();
        if(room == null)
        {
            return;
        }
        if (_grid.Buildable(x, y) && !(room is GuestRoom))
        {
            BuildingSO currentBuildingSO = room.GetBuildingSO();
            List<Vector2Int> ObjectSize = currentBuildingSO.GetObjectSize(new Vector2Int(x, y));
            foreach (Vector2Int currentGrid in ObjectSize)
            {
                GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);

                //Check if the grid is offset from the maximum or minimmun on the x or y axis
                if (gridObject == null) break;
                gridObject.SetBuilding(null);
            }
            string keyRoom = room.name;
            EconomyManager.instance.GainRevenue(currentBuildingSO.costPurchase);
            EconomyManager.instance.roomObtain.Remove(keyRoom);
            Destroy(room.gameObject);
            Debug.Log("room sold");
        }
    }
    public void BuildRoom(int x, int y, BuildingSO building)
    {
        List<Vector2Int> ObjectSize = building.GetObjectSize(new Vector2Int(x, y));
        bool canBuild = true;
        //Check whether a grid is occupied or not
        foreach (Vector2Int currentGrid in ObjectSize)
        {
            GridObject currentGridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
            //Check if the grid is offset from the maximum or minimmun on the x or y axis
            if (currentGridObject == null)
            {
                canBuild = false;
                break;
            }
            if (!_grid.GetValue(currentGrid.x, currentGrid.y).IsBuildable())
            {
                canBuild = false;
                break;

            }
        }
        //Debug.Log(EconomyManager.instance.CheckMoney(_buildingSO.costPurchase));
        if (_grid.Buildable(x, y) && canBuild && EconomyManager.instance.CheckMoney(building.costPurchase))
        {
            string roomIndexString = "room_" + room_index;
            EconomyManager.instance.UseMoney(building.costPurchase);
            Transform buildingInstantiated = Instantiate(building.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
            
            buildingInstantiated.TryGetComponent<Room>(out Room currentRoom);
            if (!(currentRoom is GuestRoom))
            {
                currentRoom.SetRoomIndex(roomIndexString);
                EconomyManager.instance.roomObtain.Add(roomIndexString, currentRoom);
                buildingInstantiated.GetComponent<SpriteRenderer>().sprite = building.roomType[x];
            }
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
    public void SetBuildingSO(BuildingSO buildingSO)
    {
        this.building = buildingSO;
    }
    public void SetGridObject(Room room)
    {
        this.room = room;
    }
    public override void Hovering()
    {
        base.Hovering();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero, 100f, interractedLayer);
        Debug.Log("Build Mode: Room "+hitInfo.collider != null);

        //If it hits something
        if (hitInfo.collider != null)
        {
            if (hitInfo.transform.TryGetComponent<Room>(out Room _currentRoom))
            {
                //If it not the same, assign it
                if (room != _currentRoom)
                {
                    SetGridObject(_currentRoom);
                }
            }
            else SetGridObject(null);
        }
        else SetGridObject(null);
    }
}
