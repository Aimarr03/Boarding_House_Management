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
        if (GameManager.instance.GameOverStatus || GameManager.instance.GameIsPaused) return;
        if (Enabled)
        {
            Hovering();
            OnClick();
        }
    }
    public void FixedUpdate()
    {
        if (Enabled && !(GameManager.instance.GameOverStatus))
        {
            OnHolding();
        }
    }
    public override void OnHolding()
    {
        base.OnHolding();
        if (Input.GetMouseButton(0) && interacting && _currentInterractedObject != null)
        {
            _currentInterractedObject.HoldInterraction();
        }
    }
    public override void OnClick()
    {
        base.OnClick();
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentInterractedObject != null && _currentInterractedObject.GetInterractState())
            {
                interacting = true;
                _currentInterractedObject.Interracted();
            }
        }
        
        if (Input.GetMouseButtonUp(0) && _currentInterractedObject != null && _currentInterractedObject.GetInterractState())
        {
            if (_currentInterractedObject is Cleaning)
            {
                Cleaning CurrentInteractedObject = _currentInterractedObject as Cleaning;
                CurrentInteractedObject.ResetDuration();
            }
            else if(_currentInterractedObject is Dialogue_Starter_Character)
            {
                _currentInterractedObject.ExitInterracted();
            }
            interacting = false;
        }
        if(Input.GetMouseButtonDown(1) && _currentInterractedObject != null)
        {
            if(_currentInterractedObject is Dialogue_Starter_Character)
            {
                Dialogue_Starter_Character currentInterractedCharacter = _currentInterractedObject as Dialogue_Starter_Character;
                currentInterractedCharacter.ToggleOccupied();
            }
        }
    }
    void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        if(_currentInterractedObject is Dialogue_Starter_Character)
        {
            if (_currentInterractedObject.GetInterractState())
            {
                _currentInterractedObject.ExitInterracted();
            }
        }
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
