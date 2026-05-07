using UnityEngine;
using UnityEngine.InputSystem;

public class MapNavigationController : MonoBehaviour
{
    public Camera cam;
    public Transform movableZone;
    public float panSpeed = 1f;
    public float zoomSpeed = 0.1f;

    public float minZoom = 3f;
    public float maxZoom = 10f;

    public Vector2 mapMinBounds;
    public Vector2 mapMaxBounds;


    private GWG_InputAction inputActions;

    private Vector2 lastTouchPosition;
    private bool isPanning, isZooming;

    private void Awake()
    {
        inputActions = new GWG_InputAction();
    }
    private void Update()
    {
        if (!isPanning) return;

        Vector2 currentPosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();

        Vector3 worldDelta =
            cam.ScreenToWorldPoint(currentPosition) -
            cam.ScreenToWorldPoint(lastTouchPosition);

        Vector3 targetPos = movableZone.position - worldDelta * panSpeed;

        movableZone.position = ClampPosition(targetPos);

        lastTouchPosition = currentPosition;
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

    //Inputs
    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.TouchPress.Enable();
        inputActions.Player.TouchPress.started += OnTouchStarted;
        inputActions.Player.TouchPress.canceled += OnTouchEnded;
    }
    private void OnDisable()
    {
        inputActions.Player.TouchPress.Disable();
        inputActions.Player.TouchPress.started -= OnTouchStarted;
        inputActions.Player.TouchPress.canceled -= OnTouchEnded;

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
}
