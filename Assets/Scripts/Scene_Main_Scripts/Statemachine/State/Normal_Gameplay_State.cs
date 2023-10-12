using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Gameplay_State : Gameplay_StateDefault
{
    private InterractableObject _currentInterractedObject;
    public static event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
    private bool interacting;
    public Normal_Gameplay_State(GameplayScene scene, Gameplay_StateMachine stateMachine) : base(scene, stateMachine)
    {
    }
    public override void EnteringState()
    {
        base.EnteringState();
        Debug.Log("Current State is Normal");
    }

    public override void Hovering()
    {
        base.Hovering();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero, 100f, interractedLayer);
        //Debug.Log(hitInfo.collider != null);

        //If it hits something
        if (hitInfo.collider != null)
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
    public override void OnClicked()
    {
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentInterractedObject != null)
            {
                interacting = true;
                _currentInterractedObject.Interracted();
            }
        }
        if (Input.GetMouseButton(0) && interacting)
        {
            _currentInterractedObject.HoldInterraction();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_currentInterractedObject is Cleaning)
            {
                Cleaning CurrentInteractedObject = _currentInterractedObject as Cleaning;
                CurrentInteractedObject.ResetDuration();
            }
            interacting = false;
        }
    }
    public void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        _currentInterractedObject = _InterractableObject;
        OnHovering?.Invoke(_currentInterractedObject);
    }

    public override void ExitingState()
    {
        base.ExitingState();
        Debug.Log("Exiting Normal State");
    }
}
