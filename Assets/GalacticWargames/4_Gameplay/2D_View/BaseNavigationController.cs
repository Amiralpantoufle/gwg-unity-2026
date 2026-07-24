using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class BaseNavigationController : IsoNavigation
{
    private GridManager gridManager;

    //Grid Options
    [SerializeField] private BaseView_Popup mainPopup;

    private Base_TileView currentlySelectedTile;
    private Base_TileView previouslySelectedTile;

    private bool tileSelected;

    protected override void Awake()
    {
        base.Awake();
        gridManager = GetComponent<GridManager>();
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

    //Selection
    public void SelectTile(Base_TileView tile)
    {
        mainPopup.Load_TileData(tile);
        tile.HighlightTile();

        currentlySelectedTile = tile;
        tileSelected = true;
    }
    public void CancelSelect()
    {
        mainPopup.Close_SelecPannel();
        currentlySelectedTile.HideTile();

        previouslySelectedTile = currentlySelectedTile;
        currentlySelectedTile = null;
        tileSelected = false;
    }
    private void TryNavigateToTile(string target)
    {
        switch (target)
        {
            case "Default":
                break;

            case "Base":
                gridManager.LoadBase(GameDataStorage.Instance.CurrentBase.base_id);
                break;
        }

    }
/*    private string IdentifyTarget()
    {
        string target = "Default";

        int selectedID = previouslySelectedTile.entity.entity_id;

        Debug.Log("identified target :" + target + "with id :" + previouslySelectedTile._Tile.entity_id + ". Compared with player base id :" + GameDataStorage.Instance.CurrentBase.base_id);

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
    }*/

    //Inputs
    protected override void OnQuickTouch(InputAction.CallbackContext ctx)
    {
        base.OnQuickTouch(ctx);

        Vector2 screenPos = base.inputActions.Player.TouchPosition.ReadValue<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);

        //UI SECURITY
        if (UICheckInput(screenPos).Count > 0)
            return;

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        if (hits == null || hits.Length == 0)
            return;


        foreach (var h in hits)
        {
            if (h.TryGetComponent<Base_TileView>(out var tile))
            {
                SelectTile(tile);
                CenterOnTile(tile.transform);
                break;
            }
        }
        // Sélection d'une tuile
    }
    protected override void OnDoubleTouch(InputAction.CallbackContext ctx)
    {
        base.OnDoubleTouch(ctx);
        // Entrer dans une base
    }

    private List<RaycastResult> UICheckInput(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = screenPos };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results;
    }
}
