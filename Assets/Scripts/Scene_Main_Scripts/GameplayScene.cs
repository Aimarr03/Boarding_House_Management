using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    private InterractableObject _currentInterractedObject;
    public static event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
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
        Debug.Log(hitInfo.collider != null);
        
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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(_currentInterractedObject != null);
            Debug.Log(_currentInterractedObject);
            if(_currentInterractedObject != null)
            {
                _currentInterractedObject.Interracted();
            }
        }
    }
    public void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        _currentInterractedObject = _InterractableObject;
        OnHovering?.Invoke(_currentInterractedObject);
    }
}
