using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private Transform gridRoot;
    [SerializeField] private Transform poolRoot;
    [SerializeField] private Sprite defaultTileSprite;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private float tileWidth = 100;
    [SerializeField] private float tileHeight = 50;
    [SerializeField] private Vector2 mapOffset;

    //Components
    private Dictionary<Vector2Int,TileView> tileViews = new Dictionary<Vector2Int, TileView>();
    [SerializeField] private EntityPool entityPool;

    public async Task GenerateMap(GridPlanetModel map)
    {
        Clear();

        List<GridTile> ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y).ToList<GridTile>();
        int total = map.tiles.Count;

        for (int i = 0; i < total; i++)
        {
            CreateTile(ordered[i]);

            if (i % 100 == 0)
            {
                LoadingScreen.Instance.loadingService.SetProgress(0.40f + (float)i / total * 0.55f,"Génération de la carte");

                await Task.Yield();
            }
        }

    }
    public async Task RenderPlanet(GridPlanetModel map)
    {
        Clear();

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        foreach (GridTile tile in ordered)
        {
            CreateTile(tile);
        }
    }
    public async Task RenderSystem(GridSystemModel map)
    {
        Clear();

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        //Generate Tile and Entities
        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }
    public async Task RenderGalaxy(GridGalaxyModel map)
    {
        Clear();

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }

    private void CreateTile(GridTile tile)
    {
        GameObject obj = Instantiate(tilePrefab);

        obj.transform.SetParent(gridRoot);
        obj.transform.position = IsoToWorld(tile.x, tile.y);
        obj.transform.name = "_"+ tile.x+"x_" +tile.y + "y";

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("Couldn't load sprite renderer");

        VisualDefinition visual = GridVisualService.Instance.GetVisual(tile.image_id);

        //Define Tileview properties and offset
        int layerOffset=0;
        if(visual.renderScale > 1) layerOffset = 10;
        sr.sortingOrder = (8000 - (tile.x + tile.y))+ layerOffset;

        sr.sprite = visual.imageSprite;
        obj.transform.localScale = Vector3.one * visual.renderScale;

        Vector2 tilePosition = new Vector2(obj.transform.localPosition.x + visual.offset.x, obj.transform.localPosition.y + visual.offset.y);
        obj.transform.localPosition = tilePosition;

        //Create Tileview
        TileView tileView = obj.GetComponent<TileView>();
        tileView.Init(tile, visual.renderScale);
        Vector2Int coords = new Vector2Int(tile.x, tile.y);
        tileViews.Add(coords, tileView);


        //Generate Entities
        if (tile.entities.Count > 0)
            entityPool.Spawn(tile.entities[0], tilePosition);
    }

    //UTILITY
    /// <summary>
    /// Recupere une tileview depuis ses coordonnées X Y
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
        float worldX = mapOffset.x + (x - y) * tileWidth * 0.5f;
        float worldY = mapOffset.y + ((x + y) * tileHeight * 0.5f);

        return new Vector3(worldX, worldY,0);
    }
    private void Clear()
    {

        foreach (Transform child in gridRoot)
            Destroy(child.gameObject);

        tileViews.Clear();
    }

}