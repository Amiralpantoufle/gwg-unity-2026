using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FleetNavigationController : IsoNavigation
{
    [SerializeField] private Fleet_GridManager gridManager;
    private Fleet_ShipView selectedShip;

    //Drag&Drop
    private bool isDragging;
    private Vector2Int originalCoords;

    protected override void Update()
    {
        base.Update();

        if (isDragging && selectedShip != null)
            MoveShip();
    }
    private void SelectShip(Fleet_ShipView ship)
    {
        selectedShip = ship;
        originalCoords = ship.CurrentCoords;
        isDragging = true;
        freezed = true;

        Debug.Log($"Vaisseau sélectionné : {ship.Ship.name}");
    }
    private void MoveShip()
    {
        Vector3 mouseWorldPosition = GetPointerWorldPosition();
        selectedShip.transform.position = mouseWorldPosition;
    }
    private void ReleaseShip()
    {
        if (selectedShip == null)
            return;

        Vector3 mouseWorldPosition = GetPointerWorldPosition();

        bool validMove = false;

        if (gridManager.TryGetTileAtWorldPosition(mouseWorldPosition,out TileView targetTile))
        {
            Vector2Int coords = new Vector2Int(targetTile._Tile.x, targetTile._Tile.y);
            validMove = gridManager.TryMoveShip(selectedShip, coords);
        }

        if (!validMove)
        {
            gridManager.PositionShipOnTile(selectedShip, originalCoords);
        }

        isDragging = false;
        selectedShip = null;
        freezed = false;
    }

    //Utility
    private void TrySelectShip()
    {
        Vector2 mousePosition = inputActions.Player.TouchPosition.ReadValue<Vector2>();
        Vector3 worldPosition = cam.ScreenToWorldPoint(mousePosition);

        if (!gridManager.TryGetTileAtWorldPosition(worldPosition,out TileView tile))
            return;

        Vector2Int coords = new Vector2Int(tile._Tile.x, tile._Tile.y); 
        if (!gridManager.TryGetShipAtTile(coords, out Fleet_ShipView ship))
            return;

        SelectShip(ship);
    }

    private Vector3 GetPointerWorldPosition()
    {
        Vector2 screenPosition =
            inputActions.Player.TouchPosition.ReadValue<Vector2>();

        Vector3 worldPosition =
            cam.ScreenToWorldPoint(
                new Vector3(
                    screenPosition.x,
                    screenPosition.y,
                    -cam.transform.position.z
                )
            );

        return worldPosition;
    }
    //Inputs
    protected override void OnTouchStarted(InputAction.CallbackContext ctx)
    {
        base.OnTouchStarted(ctx);

        if(!isDragging)
            TrySelectShip();
    }
    protected override void OnTouchEnded(InputAction.CallbackContext ctx)
    {
        base.OnTouchEnded(ctx);

        if (isDragging)
            ReleaseShip();
    }
}
