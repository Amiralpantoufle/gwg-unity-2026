using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


public class MapNavigationController : MonoBehaviour
{
    public Camera cam;
    public Transform movableZone;
    public float panSpeed = 1f;

    public Vector2 mapMinBounds;
    public Vector2 mapMaxBounds;

    public float zoomSpeed = 0.1f;
    public float minZoom = 5f;
    public float maxZoom = 1f;

    private GWG_InputAction inputActions;

    private Vector2 lastTouchPosition;
    private float lastPinchDistance;
    private bool isPanning, isZooming;

    private void Awake()
    {
        inputActions = new GWG_InputAction();
        EnhancedTouchSupport.Enable();
    }
    private void Update()
    {
        HandlePan();

#if UNITY_EDITOR || UNITY_STANDALONE
        HandlePCZoom();
#else
    HandleMobileZoom();
#endif
        HandleZoom();
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
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1) return;
        Vector2 currentPosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();

        Vector3 worldDelta = cam.ScreenToWorldPoint(currentPosition) - cam.ScreenToWorldPoint(lastTouchPosition);

        Vector3 targetPos = movableZone.position + worldDelta*panSpeed;

        movableZone.position = ClampPosition(targetPos);
        //movableZone.position = targetPos;

        lastTouchPosition = currentPosition;
    }
    private void HandleZoom()
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
        Debug.Log("Touch start");
        isPanning = true;
        lastTouchPosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();
    }
    private void OnTouchEnded(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch stop");
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
}
