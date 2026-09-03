using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private Sprite defaultTileSprite;

    [Header("WorldMap")]
    [SerializeField] private Transform gridRoot;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private EntityPool entityPool;

    [Header("Base Map")]
    [SerializeField] private Transform baseGridRoot;
    [SerializeField] private GameObject baseTilePrefab;
    [SerializeField] private EntityPool baseEntityPool;

    [SerializeField] private int tileLayerStart=10000;

    [SerializeField] private float tileWidth = 100;
    [SerializeField] private float tileHeight = 50;
    [SerializeField] private Vector2 mapOffset;

    //Grid settings
    private Vector2Int gridCenter;

    //Components
    private Dictionary<Vector2Int,TileView> tileViews = new Dictionary<Vector2Int, TileView>();
    private Dictionary<Vector2Int,Base_TileView> baseTileViews = new Dictionary<Vector2Int, Base_TileView>();

    public async Task GenerateMap(GridPlanetModel map)
    {
        Clear();

        int gridSize = map.tile_count;
        int mapWidth = (int)(Mathf.Sqrt(gridSize));
        gridCenter = new Vector2Int(mapWidth / 2, mapWidth / 2);

        for (int i = 0; i < gridSize; i++)
        {
            CreateTile(map.tiles[i]);

            if (i % 100 == 0)
            {
                LoadingScreen.Instance.loadingService.SetProgress(0.40f + (float)i / gridSize * 0.55f,"Génération de la carte");

                await Task.Yield();
            }
        }

    }
    public void RenderSystem(GridSystemModel map)
    {
        Clear();

        gridCenter = new Vector2Int(map.center_x, map.center_y);

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        //Generate Tile and Entities
        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }
    public void RenderGalaxy(GridGalaxyModel map)
    {
        Clear();

        gridCenter = new Vector2Int(map.height/2, map.width/2);

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        foreach (var tile in ordered)
        {
            CreateTile(tile);
        }
    }
    public void GenerateBase(GridBaseModel map)
    {
        Clear();

        gridCenter = new Vector2Int(map.width / 2, map.height / 2);

        var ordered = map.tiles.OrderBy(t => t.x + t.y).ThenBy(t => t.y);

        foreach (var tile in ordered)
        {
            CreateBaseTile(tile);
        }
    }

    private void CreateTile(GridTile tile)
    {
        GameObject obj = Instantiate(tilePrefab, gridRoot);

        //obj.transform.SetParent(gridRoot);
        obj.transform.position = IsoToWorld(tile.x, tile.y);
        obj.transform.name = + tile.x+"x_" +tile.y + "y";

        //Define Tileview properties and offset
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("Couldn't load sprite renderer");
        VisualDefinition visual = GridVisualService.Instance.GetVisual(tile.v);

        //Define Tileview properties and offset
        int layerOffset =0;
        if(visual.renderScale > 1) layerOffset = 10;
        sr.sortingOrder = (tileLayerStart - (tile.x + tile.y))+ layerOffset;

        sr.sprite = visual.imageSprite;
        obj.transform.localScale = Vector3.one * visual.renderScale;

        Vector2 tilePosition = new Vector2(obj.transform.position.x + visual.offset.x, obj.transform.position.y + visual.offset.y);
        obj.transform.position = tilePosition;

        //Create Tileview
        TileView tileView = obj.GetComponent<TileView>();
        tileView.Init(tile, visual.renderScale);
        Vector2Int coords = new Vector2Int(tile.x, tile.y);
        tileViews.Add(coords, tileView);


        //Generate Entities
        if (tile.entities != null && tile.entities.Count > 0)
        {
            Debug.Log($"Entity {tile.entities[0].type} on ({tile.x},{tile.y})");
            entityPool.SpawnMapEntity(tile.entities[0], obj.transform.position);
        }

    }
    private void CreateBaseTile(GridBaseTile tile)
    {
        GameObject obj = Instantiate(baseTilePrefab, baseGridRoot);

        //obj.transform.SetParent(baseGridRoot);
        obj.transform.position = IsoToWorld(tile.x, tile.y);
        obj.transform.name = "_" + tile.x + "x_" + tile.y + "y";

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("Couldn't load sprite renderer");
        VisualDefinition visual = GridVisualService.Instance.GetVisual(tile.v);

        //Define Tileview properties and offset
        int layerOffset = 0;
        if (visual.renderScale > 1) layerOffset = 10;
        sr.sortingOrder = (tileLayerStart - (tile.x + tile.y)) + layerOffset;

        sr.sprite = visual.imageSprite;
        obj.transform.localScale = Vector3.one * visual.renderScale;

        Vector2 tilePosition = new Vector2(obj.transform.position.x + visual.offset.x, obj.transform.position.y + visual.offset.y);
        obj.transform.position = tilePosition;

        //Create Tileview
        Base_TileView tileview = obj.GetComponent<Base_TileView>();
        tileview.Init(tile, visual.renderScale);
        Vector2Int coords = new Vector2Int(tile.x, tile.y);
        baseTileViews.Add(coords, tileview);

        //Generate Entities
        if (tile.entities != null && tile.entities.Count > 0)
        {
            Debug.Log(tile.entities[0].name);
            baseEntityPool.SpawnBaseEntity(tile.entities[0], tilePosition);
        }
    }

    //UTILITY
    public TileView GetTile(int x, int y)
    {
        Vector2Int coords = new Vector2Int(x, y);

        if (tileViews.TryGetValue(coords, out TileView tile))
            return tile;

        return null;
    }
    public Base_TileView GetBaseTile(int x, int y)
    {
        Vector2Int coords = new Vector2Int(x, y);

        if (baseTileViews.TryGetValue(coords, out Base_TileView tile))
            return tile;

        return null;
    }
    public Transform GetCenterTile(bool globalMap)
    {
        if(globalMap)
        {
            if (tileViews.TryGetValue(gridCenter, out TileView tile))
            {

                return tile.transform;
            }
            else
            {
                Debug.LogWarning("No tile referene to center map on");

                return null;
            }
        }
        else
        {
            if (baseTileViews.TryGetValue(gridCenter, out Base_TileView tile))
            {

                return tile.transform;
            }
            else
            {
                Debug.LogWarning("No tile referene to center map on");

                return null;
            }
        }
    }
    private Vector3 IsoToWorld(int x, int y)
    {
        float worldX = mapOffset.x + (x - y) * tileWidth * 0.5f;
        float worldY = mapOffset.y + ((x + y) * tileHeight * 0.5f);

        return new Vector3(worldX, worldY,0);
    }
    private void Clear()
    {
        //Clear Tiles
        foreach (Transform child in gridRoot)
            Destroy(child.gameObject);
        tileViews.Clear();

        foreach (Transform child in baseGridRoot)
            Destroy(child.gameObject);
        baseTileViews.Clear();


        //Clear Entities
        entityPool.ResetAllEntities();
        baseEntityPool.ResetAllEntities();
    }
}