
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class MapNavigationController : IsoNavigation
{
    private GridManager gridManager;

    //Grid Options
    [SerializeField] private MainView_TileAccess_Popup tileAccess_Popup;
    [SerializeField] private Tile_Selector tileSelector;

    private TileView currentlySelectedTile;
    private TileView previouslySelectedTile;
    private bool tileSelected;

    protected override void Awake()
    {
        base.Awake();
        gridManager = GetComponent<GridManager>();
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

        tileSelector.HighlightTile(0, tile.transform.position);
    }
    public void CancelSelect()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = tileAccess_Popup
        });
        tileSelector.Disable_Selector();

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
                gridManager.LoadBase(GameDataStorage.Instance._CurrentBase.base_id);
                break;
        }

    }
    private string IdentifyTarget()
    {
        string target = "Default";

        //Entity found
        if (previouslySelectedTile._Tile.entities != null)
        {
            int selectedID = previouslySelectedTile._Tile.entities[0].id;//Preivously .entity_id

            //Si ID correspond à une base joueur
            if (selectedID == GameDataStorage.Instance._CurrentBase.base_id)
            {
                Debug.Log("identified target :" + target + "with id :"+ previouslySelectedTile._Tile.entity_id + ". Compared with player base id :"+ GameDataStorage.Instance._CurrentBase.base_id);
                target = "Base";
            }
        }
        //Empty Tile found
        else
        {
            target = "Base";//TEMP
        }


        return target;
    }


    //Inputs
    protected override void OnQuickTouch(InputAction.CallbackContext context)
    {
        base.OnQuickTouch(context);

        if (tileSelected)
        {
            CancelSelect();
            return;
        }


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
            if (h.TryGetComponent<TileView>(out var tile))
            {
                SelectTile(tile);
                CenterOnTile(tile.transform);
                break;
            }
        }
    }
    protected override void OnDoubleTouch(InputAction.CallbackContext context)
    {
        base.OnDoubleTouch(context);

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
