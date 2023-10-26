using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingState : StateDefault
{
    public static BuildingState instance;

    [SerializeField] private Transform BuildableRoom;
    [SerializeField] private BuildingSO building;
    [SerializeField] private GuestRoomSO _guestRoomSO;
    [SerializeField] private LayerMask interractedLayer;

    [SerializeField] private _MainOption mainOption;
    [SerializeField] private Transform BuildingDialogue;
    [SerializeField] private Room room;

    [Header("Guest Room")]
    [SerializeField] private GuestRoom guestRoom;
    private CustomGrid<GridObject> _grid;
    

    private int room_index = 0;
    public void Awake()
    {
        if (instance != null) return; 
        mainOption.gameObject.SetActive(false);
        instance = this;
    }
    public void Start()
    {
        _grid = new CustomGrid<GridObject>(3, 5, 6, 3, new Vector3(-15, -5, 0), () => new GridObject());
        _grid.GetSize(out int width, out int height);
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Transform visualBuilding = Instantiate(BuildableRoom,_grid.GetMiddleWorldPosition(x,y),Quaternion.identity);
                visualBuilding.gameObject.SetActive(false);
                _grid.GetValue(x,y).SetVisualBuilding(visualBuilding);
            }
        }
        BuildRoom(0, 0, _guestRoomSO);
        SetVisualGrid(false);
    }
    public override void EnterState()
    {
        base.EnterState();
        TimeManager.instance.Pause();
        mainOption.gameObject.SetActive(true);
        mainOption.DisplayOption(0);
        CheckBuildableVisual();
        Enabled = true;
        NormalState.instance.ExitState();
    }

    public override void ExitState()
    {
        base.ExitState();
        TimeManager.instance.NormalSpeed();
        mainOption.gameObject.SetActive(false);
        SetVisualGrid(false);
        Enabled = false;
        NormalState.instance.EnterState();
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
    //Check if the lower floor already built or not
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
            if(y < height-1)
            {
                Debug.Log(!_grid.GetValue(x, y + 1).IsBuildable());
                return !_grid.GetValue(x, y + 1).IsBuildable();
            }
        }
        return false;
    }
    public void SellRoom(int x, int y)
    {
        GridObject currentObject = _grid.GetValue(x, y);
        currentObject.GetBuilding().gameObject.TryGetComponent<Room>(out Room currentRoom);
        RoomSlot currentRoomSlot = currentRoom.getRoomSlot();
        if(currentObject.IsBuildable() || !currentRoomSlot.isEmpty())
        {
            return;
        }
        room = currentObject.GetBuilding().GetComponent<Room>();
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
            string keyRoom = room.GetRoomIndex();
            EconomyManager.instance.GainRevenue(currentBuildingSO.costPurchase);
            EconomyManager.instance.roomObtain.Remove(keyRoom);
            //Remove from LinkedList
            for(int index = 0; index < EconomyManager.instance.roomObtainList.Count; index++)
            {
                if (EconomyManager.instance.roomObtainList[index].GetRoomIndex() == keyRoom)
                {
                    EconomyManager.instance.roomObtainList.RemoveAt(index);
                    Debug.Log("Remove from List" + room.GetRoomIndex());
                }
            }
            Destroy(room.gameObject);
            SetVisualBuildBuy(x, y, true);
            Debug.Log("room sold");
        }
    }
    public void ToggleEnable()
    {
        Enabled = !Enabled;
        if (Enabled)
        {
            EnterState();
        }
        else
        {
            ExitState();
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
                EconomyManager.instance.roomObtainList.Add(currentRoom);
                buildingInstantiated.GetChild(2).GetComponent<SpriteRenderer>().sprite = building.roomType[x];
            }
            else
            {
                guestRoom = currentRoom as GuestRoom;
            }
            foreach (Vector2Int currentGrid in ObjectSize)
            {
                GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);

                //Check if the grid is offset from the maximum or minimmun on the x or y axis
                if (gridObject == null) break;
                gridObject.SetBuilding(buildingInstantiated);
                SetVisualBuildBuy(x, y, false);
            }
            _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
            room_index++;
            SetVisualBuildBuy(x,y, false);
            room = null;
        }
    }
    public bool BuyFurnitue(FurnitureSO furniture)
    {
        if (EconomyManager.instance.CheckMoney(furniture.cost))
        {
            guestRoom.AddedFurniture(furniture);
            EconomyManager.instance.UseMoney(furniture.cost);
            return true;
        }
        else
        {
            return false;
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
    public void CheckBuildableVisual()
    {
        _grid.GetSize(out int width, out int height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid.GetValue(x, y).SetVisual(CheckBelow(x,y));
            }
        }
    }
    public void SetVisualBuildBuy(int x, int y, bool input)
    {
        _grid.GetSize(out int width, out int height);
        GridObject gridObject =_grid.GetValue(x, y);
        gridObject.SetVisual(input);
        if(y == height-1)
        {
            return;
        }
        if(!CheckAbove(x, y))
        {
            _grid.GetValue(x, y+1).SetVisual(!input);
        }
    }
    public void SetVisualGrid(bool input)
    {
        _grid.GetSize(out int width, out int height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid.GetValue(x, y).SetVisual(input);
            }
        }
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
