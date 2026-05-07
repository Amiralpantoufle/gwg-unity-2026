using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private Transform gridRoot;
    [SerializeField] private Sprite defaultTileSprite;

    [SerializeField] private float tileWidth = 100;
    [SerializeField] private float tileHeight = 50;

    public void Init_GridRenderer()
    {
        Debug.Log("Init GridRenderer");

        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager NULL");
            return;
        }

        GridManager.Instance.OnGridLoaded += Render;
    }
    private void Render(GridLevel level, GridMapResponse map)
    {
        Debug.Log($"Render {level}");

        Clear();

        var ordered = map.tiles
            .OrderBy(t => t.x + t.y)
            .ThenBy(t => t.y);

        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }

    private void CreateTile(GridTileDto tile)
    {
        GameObject go = new GameObject($"Tile_{tile.x}_{tile.y}");

        go.transform.SetParent(gridRoot);
        go.transform.position = IsoToWorld(tile.x, tile.y);

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = defaultTileSprite;
        sr.sortingOrder = 1000 -(tile.x + tile.y);

        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        TileView tileView = go.AddComponent<TileView>();

        tileView.Init(tile);
    }

    private Vector3 IsoToWorld(int x, int y)
    {
        float worldX = (x - y) * tileWidth * 0.5f;
        float worldY = (x + y) * tileHeight * 0.5f;

        return new Vector3(worldX, worldY,0);
    }

    private void Clear()
    {
        foreach (Transform child in gridRoot)
            Destroy(child.gameObject);
    }
}