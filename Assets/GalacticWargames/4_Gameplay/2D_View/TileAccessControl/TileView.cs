using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private GridTile tile;
    public GridTile _Tile {  get { return tile; } }

    private SpriteRenderer spriteRenderer;
    //public GridEntityModel entity;
    public void Init(GridTile data, float renderScale)
    {
        tile = data;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = collider.size / renderScale;
    }
}