using UnityEngine;

public class Base_TileView : MonoBehaviour
{
    [SerializeField] private GridBaseTile tile;
    public GridBaseTile _Tile { get { return tile; } }

    private SpriteRenderer spriteRenderer;
    public BaseEntity entity;
    public void Init(GridBaseTile data, float renderScale)
    {
        tile = data;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = collider.size / renderScale;
    }


    //Accessor
    public void HighlightTile()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
    public void HideTile()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
