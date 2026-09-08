using UnityEngine;

public class Fleet_ShipView : MonoBehaviour
{
    public FleetShip Ship { get; private set; }
    public Vector2Int CurrentCoords { get; private set; }
    public Vector2 VisualOffset { get; private set; }

    public void Init(FleetShip ship, Vector2Int coords, Vector2 visualOffset)
    {
        Ship = ship;
        CurrentCoords = coords;
        VisualOffset = visualOffset;
    }

    public void SetCoords(Vector2Int coords)
    {
        CurrentCoords = coords;
    }
}
