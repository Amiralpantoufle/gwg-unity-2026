using UnityEngine;

public class TileView : MonoBehaviour
{
    private GridTileDto tile;

    public void Init(GridTileDto data)
    {
        tile = data;

        // Rien pour l’instant
        // Le visuel sera géré via Unity Editor
    }

    private void OnMouseDown()
    {
        Debug.Log($"Click {tile.entity_id}");

        //Navigue vers l'instance index
        //GridManager.Instance.Load(GridLevel.Planet, tile.entity_id);
    }
}