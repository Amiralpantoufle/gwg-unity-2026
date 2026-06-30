using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private GridTile tile;
    public int id_esp;

    private SpriteRenderer spriteRenderer;
    public void Init(GridTile data, float renderScale)
    {
        tile = data;
        id_esp = data.entity_id;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = collider.size / renderScale;
    }


    //Accessor
    public void HighlightTile()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
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