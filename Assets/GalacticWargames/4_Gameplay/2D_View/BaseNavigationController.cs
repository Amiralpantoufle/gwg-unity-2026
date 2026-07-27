using System.Collections.Generic;
using System.Linq.Expressions;
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

    //Selection
    private void SelectTile(Base_TileView tile)
    {
        freezed = true;

        mainPopup._SelectionPannel.Load_TileData(tile);
        mainPopup._SelectionPannel.OnClosePannel += CancelSelect;

        currentlySelectedTile = tile;
        tileSelected = true;
    }
    private void CancelSelect()
    {
        freezed = false;

        previouslySelectedTile = currentlySelectedTile;
        currentlySelectedTile = null;
        tileSelected = false;

        mainPopup._SelectionPannel.OnClosePannel -= CancelSelect;
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
