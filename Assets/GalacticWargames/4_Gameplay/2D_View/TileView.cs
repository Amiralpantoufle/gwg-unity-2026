using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private GridTile tile;

    public void Init(GridTile data)
    {
        tile = data;
    }

    private void OnMouseDown()
    {
        Debug.Log($"Click {tile.image_id}");

        //Navigue vers l'instance index
        //GridManager.Instance.Load(GridLevel.Planet, tile.entity_id);
    }
}