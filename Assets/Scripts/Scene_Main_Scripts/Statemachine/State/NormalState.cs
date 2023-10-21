using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : StateDefault
{
    public static NormalState instance;
    private InterractableObject _currentInterractedObject;
    public event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
    private bool interacting;
    public void Awake()
    {
        if (instance != null) return;
        instance = this;
        Enabled = true;
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
        //Debug.Log(Enabled);
        if (Enabled)
        {
            OnClick();
            Hovering();
        }
    }
    public override void OnClick()
    {
        base.OnClick();
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentInterractedObject != null)
            {
                interacting = true;
                _currentInterractedObject.Interracted();
            }
        }
        if (Input.GetMouseButton(0) && interacting && _currentInterractedObject != null)
        {
            _currentInterractedObject.HoldInterraction();
        }
        if (Input.GetMouseButtonUp(0) && _currentInterractedObject != null)
        {
            if (_currentInterractedObject is Cleaning)
            {
                Cleaning CurrentInteractedObject = _currentInterractedObject as Cleaning;
                CurrentInteractedObject.ResetDuration();
            }
            interacting = false;
        }

    }
    void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        _currentInterractedObject = _InterractableObject;
        OnHovering?.Invoke(_currentInterractedObject);
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
}
