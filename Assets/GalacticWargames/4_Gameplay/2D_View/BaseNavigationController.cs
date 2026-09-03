using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class BaseNavigationController : IsoNavigation
{
    public static BaseNavigationController Instance;

    //Grid Options
    [SerializeField] private BaseView_Popup mainPopup;
    [SerializeField] private Tile_Selector tileSelector;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        base.Awake();
    }

    //Selection
    private void SelectTile(Base_TileView tile)
    {
        freezed = true;
        mainPopup.OnClosePannel += CancelSelect;

        buildingList building = buildingList.EmptySlot;

        //Load Entity
        if (tile._Tile.entities != null)
            building = GetTileStatus(tile._Tile.entities[0]);

        mainPopup.Open_BuildingPannel(building, tile);

        //Display Tile Selector
        if (tile._Tile.constructible)
            tileSelector.HighlightTile(0, tile.transform.position);
        else
            tileSelector.HighlightTile(1, tile.transform.position);
    }
    private void CancelSelect()
    {
        freezed = false;
        tileSelector.Disable_Selector();

        mainPopup.OnClosePannel -= CancelSelect;
    }

    //Utility
    private buildingList GetTileStatus(BaseEntity entity)
    {
        //Return tile entity
        buildingList typeOf = buildingList.EmptySlot;

        int id = entity.building_id;
        name = entity.name;

        //Chantier Spatial
        if (id >= 1 && id <= 3)
        {
            typeOf = buildingList.ChantierSpatial;
        }
        //Stockage
        else if (id >= 4 && id <= 6)
        {
            typeOf = buildingList.EspaceStockage;
        }
        //Mine Carbone
        else if (id >= 25 && id <= 27)
        {
            typeOf = buildingList.MineCarbon;
        }
        //Mine Hydrogen
        else if (id >= 28 && id <= 30)
        {
            typeOf = buildingList.MineHydrogen;
        }
        //Mine Pierre energetique
        else if (id >= 31 && id <= 33)
        {
            typeOf = buildingList.MinePierre;
        }
        else if(id >= 13 && id <=15)
        {
            typeOf = buildingList.Undefined;
        }

        return typeOf;
    }

    //Inputs
    protected override void OnQuickTouch(InputAction.CallbackContext ctx)
    {
        base.OnQuickTouch(ctx);

        if (freezed) return;

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

        if (freezed) return;

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
