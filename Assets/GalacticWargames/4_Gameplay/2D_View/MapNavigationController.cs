
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class MapNavigationController : MonoBehaviour
{
    private GWG_InputAction inputActions;
    private GridManager gridManager;

    //Camera settings
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

    //Navigation
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

    //Camera
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

    //Inputs
    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.TouchPress.started += OnTouchStarted;
        inputActions.Player.TouchPress.canceled += OnTouchEnded;

        inputActions.Player.Pinch.started += OnPinchStarted;
        inputActions.Player.Pinch.canceled += OnPinchEnded;

        inputActions.Player.QuickTouch.performed += OnQuickTouch;
    }
    private void OnDisable()
    {
        inputActions.Player.TouchPress.started -= OnTouchStarted;
        inputActions.Player.TouchPress.canceled -= OnTouchEnded;

        inputActions.Player.Pinch.started -= OnPinchStarted;
        inputActions.Player.Pinch.canceled -= OnPinchEnded;

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
        if (gridManager._TileSelected)
        {
            gridManager.CancelSelect();
            return;
        }

        Vector2 screenPos = inputActions.Player.TouchPosition.ReadValue<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        if (hits == null || hits.Length == 0)
            return;


        // 1. UI override
        foreach (var h in hits)
        {
            if (h.gameObject.layer == 5)//5:UI
                return;

        }

        // 2. Tile priority
        foreach (var h in hits)
        {
            if (h.TryGetComponent<TileView>(out var tile))
            {
                gridManager.SelectTile(tile);
                CenterOnTile(tile.transform);
                return;
            }
        }
    }
}
