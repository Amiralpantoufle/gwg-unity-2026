
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class MapNavigationController : MonoBehaviour
{
    private GWG_InputAction inputActions;
    private GridManager gridManager;

    //Grid Options
    [SerializeField] private MainView_TileAccess_Popup tileAccess_Popup;
    private TileView currentlySelectedTile;
    private TileView previouslySelectedTile;
    private bool tileSelected;

    //Camera Navigation
    public Camera cam;
    public Transform movableZone;
    public float panSpeed = 1f;
    [SerializeField] private float centerSmoothTime = 1f;
    private Vector3 centeringTarget;
    private Vector3 velocity;
    private bool isCentering;

    public Vector2 mapMinBounds;
    public Vector2 mapMaxBounds;

    public float zoomSpeed = 0.1f;
    public float minZoom = 5f;
    public float maxZoom = 1f;

    private Vector2 lastTouchPosition;
    private float lastPinchDistance;
    private bool isPanning, isZooming;

    private void Awake()
    {
        inputActions = new GWG_InputAction();
        EnhancedTouchSupport.Enable();

        gridManager = GetComponent<GridManager>();
        if (gridManager == null) Debug.LogError("NoGridManager Attached to GameObject");
    }
    private void Update()
    {
        HandlePan();
        HandleCentering();

#if UNITY_EDITOR || UNITY_STANDALONE
        HandlePCZoom();
#else
        HandleMobileZoom();
#endif

    }
    private void SetBoundariesFromMap()
    {
        /* mapMinBounds = new Vector2(0, 0);
         mapMaxBounds = new Vector2(mapWidth, mapHeight);

         float worldWidth = gridWidth * tileWidth;
 float worldHeight = gridHeight * tileHeight;

 mapMaxBounds = new Vector2(worldWidth, worldHeight);

         */
    }
    Vector3 ClampPosition(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        float minX = mapMinBounds.x + camWidth;
        float maxX = mapMaxBounds.x - camWidth;

        float minY = mapMinBounds.y + camHeight;
        float maxY = mapMaxBounds.y - camHeight;

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        return targetPos;
    }

    //Camera Navigation
    private void HandlePan()
    {
        if (!isPanning) return;

        //Annuler le centrage quand panning detecte
        isCentering = false;

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1) return;
        Vector2 currentPosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();

        Vector3 worldDelta = cam.ScreenToWorldPoint(currentPosition) - cam.ScreenToWorldPoint(lastTouchPosition);

        Vector3 targetPos = movableZone.position + worldDelta*panSpeed;

        //movableZone.position = ClampPosition(targetPos);
        movableZone.position = targetPos;

        lastTouchPosition = currentPosition;
    }
    private void HandleCentering()
    {
        if (!isCentering)
            return;

        movableZone.position = Vector3.SmoothDamp(
            movableZone.position,
            centeringTarget,
            ref velocity,
            centerSmoothTime);

        if (Vector3.Distance(movableZone.position, centeringTarget) < 0.05f)
        {
            movableZone.position = centeringTarget;
            isCentering = false;

            Debug.Log("Centered");
        }
    }
    public void CenterOnTile(Transform tile)
    {
        centeringTarget = -tile.localPosition;
        isCentering = true;
    }
    private void HandleMobileZoom()
    {
        if (!isZooming) return;

        // PC Scroll
        Vector2 scroll = inputActions.Player.Pinch.ReadValue<Vector2>();

        if (scroll.y != 0)
        {
            cam.orthographicSize -= scroll.y * zoomSpeed * Time.deltaTime * 100f;
        }
    }
    private void HandlePCZoom()
    {
        if (!isZooming) return;

        // PC Scroll
        Vector2 scroll = inputActions.Player.Pinch.ReadValue<Vector2>();

        if (scroll.y != 0)
        {
            cam.orthographicSize -= scroll.y * zoomSpeed * Time.deltaTime * 100f;
        }

        // Mobile Pinch
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count >= 2)
        {
            var touch0 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            var touch1 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[1];

            float currentDistance =
                Vector2.Distance(touch0.screenPosition, touch1.screenPosition);

            if (lastPinchDistance == 0)
            {
                lastPinchDistance = currentDistance;
                return;
            }

            float delta = currentDistance - lastPinchDistance;

            cam.orthographicSize -= delta * zoomSpeed * Time.deltaTime;

            lastPinchDistance = currentDistance;
        }
        else
        {
            lastPinchDistance = 0;
        }

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize,
            maxZoom,
            minZoom
        );
    }

    //Selection
    public void SelectTile(TileView tile)
    {
        EventBus.Publish(new ShowPopupEvent
        {
            popup = tileAccess_Popup
        });
        tileAccess_Popup.LoadTileViewData(tile);

        currentlySelectedTile = tile;
        tileSelected = true;
    }
    public void CancelSelect()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = tileAccess_Popup
        });
        tileAccess_Popup.HideCurrentTile();
        tileSelected = false;

        previouslySelectedTile = currentlySelectedTile;
    }
    private void TryNavigateToTile(string target)
    {
        switch(target)
        {
            case "Default":
                break;

            case "Base":
                gridManager.LoadBase(GameDataStorage.Instance.CurrentBase.base_id);
                break;
        }

    }
    private string IdentifyTarget()
    {
        string target = "Default";

        int selectedID = previouslySelectedTile.entity.entity_id;

        Debug.Log("identified target :" + target + "with id :"+ previouslySelectedTile._Tile.entity_id + ". Compared with player base id :"+ GameDataStorage.Instance.CurrentBase.base_id);

        //Si ID correspond à une base joueur
        if (selectedID == GameDataStorage.Instance.CurrentBase.base_id)
        {
            target = "Base";
        }
        else
        {
            target = "Base";
        }

        return target;
    }
    //Inputs
    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.TouchPress.started += OnTouchStarted;
        inputActions.Player.TouchPress.canceled += OnTouchEnded;

        inputActions.Player.Pinch.started += OnPinchStarted;
        inputActions.Player.Pinch.canceled += OnPinchEnded;

        inputActions.Player.QuickTouch.performed += OnQuickTouch;
        inputActions.Player.DoubleTap.performed += OnDoubleTouch;
    }
    private void OnDisable()
    {
/*        inputActions.Player.TouchPress.started -= OnTouchStarted;
        inputActions.Player.TouchPress.canceled -= OnTouchEnded;

        inputActions.Player.Pinch.started -= OnPinchStarted;
        inputActions.Player.Pinch.canceled -= OnPinchEnded;*/

        inputActions.Player.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext ctx)
    {
        isPanning = true;
        lastTouchPosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();
    }
    private void OnTouchEnded(InputAction.CallbackContext ctx)
    {
        isPanning = false;
    }

    private void OnPinchStarted(InputAction.CallbackContext context)
    {
        isZooming = true;
    }
    private void OnPinchEnded(InputAction.CallbackContext context)
    {
        isZooming = false;
    }

    private void OnQuickTouch(InputAction.CallbackContext context)
    {
        if (tileSelected)
        {
            CancelSelect();
            return;
        }


        Vector2 screenPos = inputActions.Player.TouchPosition.ReadValue<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);

        //UI SECURITY
        if (UICheckInput(screenPos).Count > 0)
            return;


        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        if (hits == null || hits.Length == 0)
            return;


        foreach (var h in hits)
        {
            if (h.TryGetComponent<TileView>(out var tile))
            {
                SelectTile(tile);
                CenterOnTile(tile.transform);
                break;
            }
        }
    }
    private void OnDoubleTouch(InputAction.CallbackContext context)
    {
        if (previouslySelectedTile != null)
            TryNavigateToTile(IdentifyTarget());
    }

    private List<RaycastResult> UICheckInput(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = screenPos };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results;
    }
}
