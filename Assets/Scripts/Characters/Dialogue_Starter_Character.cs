using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Starter_Character : InterractableObject
{
    [SerializeField] private LayerMask interractedLayer;
    public Character character;
    private bool occupied;
    private bool _isDragging;
    Vector2 _originalPosition, _offset;

    private Room currentRoom;
    //For Searching Room using mouse
    Room RoomSearched;
    public override void Awake()
    {
        base.Awake();
        _isDragging = false;
        _originalPosition = transform.position;
        occupied = false;
    }
    public override void HoldInterraction()
    {
        if (!_isDragging) return;
        transform.position = GetMousePosition() - _offset;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero, 100f, interractedLayer);
        if(hitInfo.collider != null)
        {
            Debug.Log("hit something");
            if(hitInfo.transform.gameObject.TryGetComponent<Room>(out Room room))
            {
                RoomSearched = room;
                Debug.Log("Room Obtain ");
            }
        }
    }
    public override void Interracted()
    {
        base.Interracted();
        _isDragging = true;
        _offset = GetMousePosition() - (Vector2)transform.position;
    }
    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void OnHoverEnter()
    {
        base.OnHoverEnter();
    }

    public override void OnHoverExit()
    {
        base.OnHoverExit();
    }

    public override void ExitInterracted()
    {
        base.ExitInterracted();
        if (RoomSearched != null)
        {
            if (Vector2.Distance(RoomSearched.transform.position, transform.position) < 10f)
            {
                if (RoomSearched.getRoomSlot().isEmpty())
                {
                    //To changed from one room to another
                    if (currentRoom != null)
                    {
                        currentRoom.getRoomSlot().SetRoom(null);
                    }
                    currentRoom = RoomSearched;
                    RoomSearched.getRoomSlot().SetRoom(gameObject.GetComponent<Character>());
                    Debug.Log("Room is set");
                    return;
                }
            }

        }
        transform.position = _originalPosition;
    }

    public void UpdateOriginalPosition(Vector2 position)
    {
        _originalPosition = position;
    }
    public bool GetDrageState()
    {
        return _isDragging;
    }
    public void ToggleOccupied()
    {
        occupied = !occupied;
    }
    public void SetCurrentRoom(Room room)
    {
        this.currentRoom = room;
    }
}
