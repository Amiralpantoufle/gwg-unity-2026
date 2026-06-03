using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private Transform gridRoot;
    [SerializeField] private Sprite defaultTileSprite;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private float tileWidth = 100;
    [SerializeField] private float tileHeight = 50;

    private Dictionary<Vector2Int, TileView> tileViews = new Dictionary<Vector2Int, TileView>();
    public void Init_GridRenderer()
    {
        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager NULL");
            return;
        }

        GridManager.Instance.OnGridLoaded += Render;
    }
    private void Render(GridLevel level, GridMapResponse map)
    {
        Clear();

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }

    private void CreateTestTile(GridTileDto tile)
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
    private void CreateTile(GridTileDto tile)
    {
        GameObject obj = Instantiate(tilePrefab);

        obj.transform.SetParent(gridRoot);
        obj.transform.position = IsoToWorld(tile.x, tile.y);
        obj.transform.name = tilePrefab.name + "_"+ tile.x+"x_" +tile.y + "y";

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        VisualDefinition visual = GridVisualService.Instance.GetVisual(tile.image_id);
        if (sr == null || visual == null)
        {
            Debug.LogError("Couldn't load sprite renderer or visual definition");
            return;
        }

        //Define Tileview properties and offset
        int autoOffset=0;
        if(visual.renderScale > 1) autoOffset = 10;
        sr.sortingOrder = (1000 - (tile.x + tile.y))+autoOffset;
        sr.sprite = visual.imageSprite;
        obj.transform.localScale = Vector3.one * visual.renderScale;
        obj.transform.localPosition = new Vector2(obj.transform.localPosition.x + visual.offset.x, obj.transform.localPosition.y + visual.offset.y);

        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();

        //Create Tileview
        TileView tileView = obj.AddComponent<TileView>();
        tileView.Init(tile);

        //Register Tileview
        Vector2Int coords = new Vector2Int(tile.x, tile.y);
        tileViews.Add(coords, tileView);
    }

    /// <summary>
    /// Recupere une tileview depuis son ID de tuile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TileView GetTile(int x, int y)
    {
        Vector2Int coords = new Vector2Int(x, y);

        if (tileViews.TryGetValue(coords, out TileView tile))
            return tile;

        return null;
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