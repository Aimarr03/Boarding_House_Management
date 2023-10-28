using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private int edgeScreen;
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float bottomBorder;
    // Update is called once per frame
    void Update()
    {
        Vector2 directionCamera = GetVector2();

        
        transform.position += cameraSpeed * (Vector3)directionCamera * Time.deltaTime;
    }
    private Vector2 GetVector2()
    {
        if (transform.position.x < leftBorder) return new Vector2(1, 0);
        if(transform.position.x > rightBorder) return new Vector2(-1,0);
        if (transform.position.y < bottomBorder) return new Vector2(0, 1);
        if(transform.position.y > topBorder) return new Vector2(0,-1);
        Vector2 directionCamera = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            directionCamera.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            directionCamera.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            directionCamera.x = +1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            directionCamera.x = -1;
        }
        if (Input.mousePosition.x < edgeScreen)
        {
            directionCamera.x = -1;
        }
        if (Input.mousePosition.y < edgeScreen)
        {
            directionCamera.y = -1;
        }
        if (Input.mousePosition.x > Screen.width - edgeScreen)
        {
            directionCamera.x = +1;
        }
        if (Input.mousePosition.y > Screen.height - edgeScreen)
        {
            directionCamera.y = +1;
        }
        return directionCamera;
    }
}
