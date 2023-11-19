using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameplayScene : MonoBehaviour
{
    public enum State
    {
        Normal,
        Building,
        Dialogue
    }
    public State currentState;

    [SerializeField] public BuildingSO _buildingSO;
    private InterractableObject _currentInterractedObject;
    public static event System.Action<InterractableObject> OnHovering;
    [SerializeField] private LayerMask interractedLayer;
    private CustomGrid<GridObject> _grid;
    private bool interacting;
    public static GameplayScene _gameplayManager;

    public void Awake()
    {
        if (_gameplayManager != null) return;
        _gameplayManager = this;
    }

    private void Start()
    {

        interacting = false;
        _grid = new CustomGrid<GridObject>(3,5, 9,3, new Vector3(-15, -5, 0), () => new GridObject());

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Normal:
                HoveringNormal();
                OnclickedNormal();
                break;
            case State.Building:
                OnClickedBuild();
                break;
            case State.Dialogue:
                break;
        }
    }
    #region Normal Mode
    void SetCurrentInterractableObject(InterractableObject _InterractableObject)
    {
        _currentInterractedObject = _InterractableObject;
        OnHovering?.Invoke(_currentInterractedObject);
    }
    public void HoveringNormal()
    {
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
    public void OnclickedNormal()
    {
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
    #endregion
    #region Build Mode
    public void OnClickedBuild()
    {
        Vector3 MousePositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _grid.GetXY(MousePositionVector3, out int x, out int y);

            List<Vector2Int> ObjectSize = _buildingSO.GetObjectSize(new Vector2Int(x, y));
            bool canBuild = true;
            //Check whether a grid is occupied or not
            foreach (Vector2Int currentGrid in ObjectSize)
            {

                GridObject currentGridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                if (currentGridObject == null) break;
                if (!_grid.GetValue(currentGrid.x, currentGrid.y).IsBuildable())
                {
                    canBuild = false;
                    break;

                }
            }
            if (_grid.Buildable(x, y) && canBuild)
            {
                Transform buildingInstantiated = Instantiate(_buildingSO.prefab, _grid.GetMiddleWorldPosition(x, y), Quaternion.identity);
                foreach (Vector2Int currentGrid in ObjectSize)
                {
                    GridObject gridObject = _grid.GetValue(currentGrid.x, currentGrid.y);
                    if (gridObject == null) break;
                    gridObject.SetBuilding(buildingInstantiated);
                }
                _grid.GetValue(x, y).SetBuilding(buildingInstantiated);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(MousePositionVector3));
        }
    }
    #endregion
    public void SetState(int input)
    {
        if (input == 0) currentState = State.Normal;
        if(input == 1) currentState = State.Building;
        if(input ==2) currentState = State.Dialogue;
    }
    
}
