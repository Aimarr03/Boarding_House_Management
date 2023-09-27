using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Player_Input PlayerInputSystem;
    private float _movementSpeed;
    private void Awake()
    {
        PlayerInputSystem = GetComponent<Player_Input>();
        _movementSpeed = 5f;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 _playerMovementDirection = PlayerInputSystem.GetNormalizedVector2PlayerMovementDirection();
        Vector3 _playerVector3Direction = new Vector3(_playerMovementDirection.x, _playerMovementDirection.y, 0);
        transform.position += _playerVector3Direction * _movementSpeed * Time.deltaTime;
    }

}
