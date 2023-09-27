using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    private Player_Input_Manager _PlayerInputManager;
    private void Awake()
    {
        _PlayerInputManager = new Player_Input_Manager();
        _PlayerInputManager.Player.Movement.Enable();
    }

    public Vector2 GetNormalizedVector2PlayerMovementDirection()
    {
        Vector2 movementDirection = _PlayerInputManager.Player.Movement.ReadValue<Vector2>();
        movementDirection.Normalize();
        return movementDirection;
    }
}
