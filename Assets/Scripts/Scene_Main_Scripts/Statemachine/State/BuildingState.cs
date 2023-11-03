using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingState : StateDefault, IDataPersistance
{
    public static BuildingState instance;

    [SerializeField] private Transform BuildableRoom;
    [SerializeField] private BuildingSO building;
    [SerializeField] private GuestRoomSO _guestRoomSO;
    [SerializeField] private LayerMask interractedLayer;

    //For loading room data;
    [SerializeField] private List<BuildingSO> buildings;
    [SerializeField] private List<CharacterSO> characters;

    [SerializeField] private _MainOption mainOption;
    [SerializeField] private Transform BuildingDialogue;
    [SerializeField] private Room room;
    [SerializeField] private BuildingListSO buildingList;
    [SerializeField] private Transform boardingHouseLocation;

    [Header("Guest Room")]
    public GuestRoom guestRoom;
    private CustomGrid<GridObject> _grid;
    

    private int room_index = 0;
    public void Awake()
    {
        if (instance != null) return;
        mainOption.BuildingContainer.gameObject.SetActive(false);
        instance = this;
        Vector3 Location = boardingHouseLocation.position;
        _grid = new CustomGrid<GridObject>(3, 6, 14, 7, Location, () => new GridObject());
        _grid.GetSize(out int width, out int height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform visualBuilding = Instantiate(BuildableRoom, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                visualBuilding.gameObject.SetActive(false);
                _grid.GetValue(x, y).SetVisualBuilding(visualBuilding);
            }
        }
        SetVisualGrid(false);
        BuildRoom(0, 0, _guestRoomSO);
    }
    public override void EnterState()
    {
        base.EnterState();
        TimeManager.instance.Pause();
        TimeManager.instance.StopBackgroundChange();
        mainOption.BuildingContainer.gameObject.SetActive(true);
        mainOption.DisplayOption(0);
        CheckBuildableVisual();
        Enabled = true;
        NormalState.instance.ExitState();
    }

    public override void ExitState()
    {
        base.ExitState();
        TimeManager.instance.NormalSpeed();
        TimeManager.instance.StartBackgroundChange();
        mainOption.BuildingContainer.gameObject.SetActive(false);
        SetVisualGrid(false);
        Enabled = false;
        NormalState.instance.EnterState();
    }
    public void Update()
    {
        if (GameManager.instance.GameOverStatus || GameManager.instance.GameIsPaused) return;
        if (Enabled)
        {
            //Hovering();
            OnClick();
        }
    }
    public override void OnClick()
    {
        base.OnClick();
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && building != null)
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);
            if(CheckBelow(x,y)) BuildRoom(x, y, building);
            
            //CheckBelow(x, y, building);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Alternative Clicked!");
            _grid.GetXY(MousePositionVector3, out int x, out int y);
            Debug.Log("There is no above room: "+!CheckAbove(x, y));
            if (!CheckAbove(x,y)) SellRoom(x, y); 
            building = null;
        }
    }
    //Check if the lower floor is already built 
    public bool CheckBelow(int x, int y)
    {
        if (_grid.Buildable(x, y))
        {
            if (y > 0)
            {
                //below is built
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
                //above is built
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
            buildingInstantiated.SetParent(boardingHouseLocation, true);


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
                ManagerCharacter.instance.SetPosition(guestRoom.ManagerOriginalPosition);
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
            this.building = null;
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
    public BuildingSO GetBuildingSO()
    {
        return building;
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

    public void LoadScene(GameData gameData)
    {
        room_index = gameData.room_index;
        List<GameData.RoomData> roomDatas = gameData.roomDatas;
        foreach (GameData.RoomData roomData in roomDatas)
        {
            if(roomData.roomType != "Guest Room")
            {
                BuildingSO currentBuildingSO = GetBuildingSO(roomData.roomType);
                Transform buildingInstantiated = Instantiate(currentBuildingSO.prefab, 
                    _grid.GetMiddleWorldPosition(roomData.x_axis, roomData.y_axis), Quaternion.identity);
                _grid.GetValue(roomData.x_axis, roomData.y_axis).SetBuilding(buildingInstantiated);
                buildingInstantiated.gameObject.TryGetComponent<Room>(out Room room);
                SpriteRenderer spriteRenderer = buildingInstantiated.GetChild(2).GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = currentBuildingSO.roomType[roomData.x_axis];
                Debug.Log(room.GetRoomIndex());
                if(roomData.roomName != "")
                {
                    EconomyManager.instance.roomObtain.Add(roomData.roomName, room);
                    EconomyManager.instance.roomObtainList.Add(room);
                }
                room.SetRoomIndex(roomData.roomName);
                if (roomData.HasPlayer)
                {
                    CharacterSO characterSO = GetCharacterSO(roomData.characterName);
                    Transform character = Instantiate(characterSO.prefabCharacter.transform);
                    buildingInstantiated.gameObject.TryGetComponent<RoomSlot>(out RoomSlot roomSlot);
                    roomSlot.SetRoom(character.GetComponent<Character>());
                }
            }
        }
        
    }
    private void ClearOldData()
    {
        // Clear old room data
        foreach (var room in EconomyManager.instance.roomObtainList)
        {
            Destroy(room.gameObject);
        }
        EconomyManager.instance.roomObtainList.Clear();
        EconomyManager.instance.roomObtain.Clear();

        // Clear old furniture data
        CleanManager.instance.furnitures.Clear();
    }
    public BuildingSO GetBuildingSO(string roomName)
    {
        foreach(BuildingSO buildingSO in buildings)
        {
            if(buildingSO.roomName == roomName)
            {
                return buildingSO;
            }
        }
        return null;
    }
    public CharacterSO GetCharacterSO(string characterName)
    {
        foreach(CharacterSO characterSO in characters)
        {
            if(characterSO.characterName == characterName)
            {
                return characterSO;
            }
        }
        return null;
    }
    
    public void SaveScene(ref GameData gameData)
    {
        gameData.roomDatas.Clear();
        gameData.room_index = room_index;
        _grid.GetSize(out int  width, out int height);
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GridObject gridObject = _grid.GetValue(x, y);
                if (gridObject.IsBuildable()) continue;
                gridObject.GetBuilding().TryGetComponent<Room>(out Room room);
                gridObject.GetBuilding().TryGetComponent<RoomSlot>(out RoomSlot roomSlot);
                GameData.RoomData roomData = new GameData.RoomData();
                roomData.x_axis = x;
                roomData.y_axis = y;
                roomData.width = room.GetBuildingSO().width;
                roomData.height = room.GetBuildingSO().height;
                roomData.roomName = room.GetRoomIndex();
                roomData.roomType = room.GetBuildingSO().roomName;
                roomData.HasPlayer = !roomSlot.isEmpty();
                gameData.roomDatas.Add(roomData);
                if (roomData.HasPlayer)
                {
                    CharacterSO characterSO = roomSlot.GetCharacter().GetCharacterSO();
                    roomData.characterName = characterSO.name;
                }
            }
        }
    }
    
}
