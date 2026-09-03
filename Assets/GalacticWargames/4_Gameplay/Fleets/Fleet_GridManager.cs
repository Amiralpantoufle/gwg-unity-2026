using System.Collections.Generic;
using UnityEngine;

public class Fleet_GridManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;

    [Header("Tile")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform gridRoot;

    [Header("Isometric")]
    [SerializeField] private int tileLayerStart = 10000;
    [SerializeField] private float tileWidth = 100f;
    [SerializeField] private float tileHeight = 50f;
    [SerializeField] private Vector2 mapOffset;

    private readonly Dictionary<Vector2Int, TileView> tileViews = new();

    public void Generate_EmptyGrid(int v)
    {
        Clear();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridTile tile = CreateEmptyTile(x, y, v);
                CreateTile(tile);
            }
        }
    }
    private GridTile CreateEmptyTile(int x, int y, int v)
    {
        return new GridTile
        {
            x = x,
            y = y,
            v= v

            // À adapter selon modèle GridTile
            // entities = ...
        };
    }
    private void CreateTile(GridTile tile)
    {
        GameObject obj = Instantiate(tilePrefab, gridRoot);

        obj.name = $"{tile.x}x_{tile.y}y";
        obj.transform.position = IsoToWorld(tile.x, tile.y);

        // --------------------------------------------------
        // Visual
        // --------------------------------------------------q

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError($"Couldn't find SpriteRenderer on {obj.name}");
            return;
        }

        VisualDefinition visual = GridVisualService.Instance.GetVisual(tile.v);

        int layerOffset = visual.renderScale > 1 ? 10 : 0;

        sr.sortingOrder = tileLayerStart - (tile.x + tile.y) + layerOffset;
        sr.sprite = visual.imageSprite;

        obj.transform.localScale =  Vector3.one * visual.renderScale;

        obj.transform.position += (Vector3)visual.offset;

        // --------------------------------------------------
        // TileView
        // --------------------------------------------------

        TileView tileView = obj.GetComponent<TileView>();

        if (tileView == null)
        {
            Debug.LogError($"Couldn't find TileView on {obj.name}");
            return;
        }

        tileView.Init(tile, visual.renderScale);

        Vector2Int coords = new Vector2Int(tile.x, tile.y);

        tileViews.Add(coords, tileView);
    }
    private Vector3 IsoToWorld(int x, int y)
    {
        float worldX =mapOffset.x + (x - y) * tileWidth * 0.5f;

        float worldY =mapOffset.y +(x + y) * tileHeight * 0.5f;

        return new Vector3(worldX, worldY, 0f);
    }
    public TileView GetTile(int x, int y)
    {
        return GetTile(new Vector2Int(x, y));
    }

    public TileView GetTile(Vector2Int coords)
    {
        tileViews.TryGetValue(coords, out TileView tileView);
        return tileView;
    }

    public void Clear()
    {
        foreach (Transform child in gridRoot)
        {
            Destroy(child.gameObject);
        }

        tileViews.Clear();
    }
}
