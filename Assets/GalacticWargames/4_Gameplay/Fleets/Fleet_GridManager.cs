using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Fleet_GridManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private LineRenderer frontLine;
    private int spaceShipsLayer = 10001;

    [Header("Tile")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform gridRoot;
    [SerializeField] private GameObject offTilePrefab;
    [SerializeField] private Transform offGridRoot;

    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private Transform shipsRoot;
    private int tileV_ID;

    [Header("Isometric")]
    [SerializeField] private int tileLayerStart = 10000;
    [SerializeField] private float tileWidth = 100f;
    [SerializeField] private float tileHeight = 50f;
    [SerializeField] private Vector2 mapOffset;

    private readonly Dictionary<Vector2Int, TileView> tileViews = new();
    private readonly Dictionary<Vector2Int, Fleet_ShipView> spaceshipViews = new();

    //Grid Building
    public void Generate_EmptyGrid(int v)
    {
        tileV_ID = v;

        ClearGrid();
        ClearShips();

        Generate_PlayGrid();
        Generate_OffGrid();
        Add_FrontLine();
    }
    private void Generate_PlayGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridTile tile = CreateEmptyTile(x, y);
                CreateTile(tile);
            }
        }
    }
    private void Generate_OffGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = -1; y >= -gridHeight; y--)
            {
                CreateOffGridTile(x, y);
            }
        }
    }
    private GridTile CreateEmptyTile(int x, int y)
    {
        return new GridTile
        {
            x = x,
            y = y,
            v= tileV_ID

            // À adapter selon modèle GridTile
            // entities = ...
        };
    }
    private void CreateOffGridTile(int x, int y)
    {
        GameObject obj = Instantiate(offTilePrefab, offGridRoot);

        obj.name = $"OffGrid_{x}x_{y}y";

        obj.transform.position = IsoToWorld(x, y);

        VisualDefinition visual = GridVisualService.Instance.GetVisual(tileV_ID);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError($"Couldn't find SpriteRenderer on {obj.name}");
            return;
        }

        sr.sprite = visual.imageSprite;

        obj.transform.localScale =Vector3.one * visual.renderScale;
        obj.transform.position +=(Vector3)visual.offset;
        sr.sortingOrder = 200;
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

        // TileView
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
    private void Add_FrontLine()
    {
        if (frontLine == null)
        {
            Debug.LogWarning("FrontLine LineRenderer is not assigned.");

            return;
        }

        float frontY = -0.5f;

        float startX = -0.5f;
        float endX = gridWidth - 0.5f;

        Vector3 start = IsoToWorld(startX, frontY);
        Vector3 end = IsoToWorld(endX, frontY);

        frontLine.positionCount = 2;

        frontLine.SetPosition(0, start);
        frontLine.SetPosition(1, end);
    }

    //Ships
    public void Load_FleetShips_OnGrid(FleetShip[] composition)
    {
        Clear(shipsRoot);

        foreach (FleetShip ship in composition)
        {
            Vector2Int pos = new Vector2Int(ship.x, ship.y);
            Add_SpaceShip(ship, pos);
        }
    }
    private void Add_SpaceShip(FleetShip ship, Vector2Int coord)
    {
        VisualDefinition visual = GridVisualService.Instance.GetVisual(ship.ship_id);

        Quaternion rot = Quaternion.identity;
        if(!tileViews.TryGetValue(coord, out TileView tile))
        {
            Debug.LogError($"Impossible de placer le vaisseau : aucune tuile trouvée en {coord}");
            return;
        }

        GameObject newShip = Instantiate(shipPrefab,tile.transform.position,rot,shipsRoot);
        SpriteRenderer sr = newShip.GetComponent<SpriteRenderer>();
        sr.sprite = visual.imageSprite;
        sr.sortingOrder = spaceShipsLayer;
        newShip.transform.localScale =Vector3.one * visual.renderScale;

        // Offset visuel
        Fleet_ShipView shipView =newShip.GetComponent<Fleet_ShipView>();
        shipView.Init(ship, coord, visual.offset);
        shipView.transform.position = tile.transform.position + (Vector3)shipView.VisualOffset;
        spaceshipViews.Add(coord, shipView);
    }
    public bool PositionShipOnTile(Fleet_ShipView ship,Vector2Int coords)
    {
        if (!tileViews.TryGetValue(coords,out TileView tile))
        {
            Debug.LogError( $"Impossible de trouver la tile {coords}" );
            return false;
        }

        ship.transform.position =tile.transform.position + (Vector3)ship.VisualOffset;
        return true;
    }
    public bool TryMoveShip(Fleet_ShipView ship, Vector2Int targetCoords)
    {
        // Vérifie que la Tile existe
        if (!tileViews.TryGetValue(targetCoords,out TileView targetTile))
        {
            Debug.LogWarning( $"Impossible de déplacer le vaisseau : Tile {targetCoords} inexistante.");
            return false;
        }

        Vector2Int currentCoords = ship.CurrentCoords;

        // Pas besoin de bouger si c'est la même Tile
        if (currentCoords == targetCoords)
        {
            PositionShipOnTile(ship, currentCoords);
            return true;
        }

        // Vérifie si la destination est occupée
        if (spaceshipViews.ContainsKey(targetCoords))
        {
            Debug.Log( $"Impossible : Tile {targetCoords} déjà occupée.");
            return false;
        }

        spaceshipViews.Remove(currentCoords);
        spaceshipViews.Add(targetCoords,ship);

        // Met à jour les coordonnées du ShipView
        ship.SetCoords(targetCoords);

        // Positionne visuellement
        PositionShipOnTile(ship,targetCoords);

        return true;
    }


    //Utility
    /*private Vector3 IsoToWorld(int x, int y)
    {
        float worldX =mapOffset.x + (x - y) * tileWidth * 0.5f;
        float worldY =mapOffset.y +(x + y) * tileHeight * 0.5f;

        return new Vector3(worldX, worldY, 0f);
    }*/
    private Vector3 IsoToWorld(float x, float y)
    {
        float worldX =mapOffset.x +(x - y) * tileWidth * 0.5f;

        float worldY =mapOffset.y + (x + y) * tileHeight * 0.5f;

        return new Vector3(worldX,worldY,0f);
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
    public bool TryGetTileAtWorldPosition(Vector2 worldPosition,out TileView tileView)
    {
        Collider2D hit =Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
        {
            tileView = null;
            return false;
        }

        tileView = hit.GetComponent<TileView>();

        return tileView != null;
    }
    public bool TryGetShipAtTile(Vector2Int coords,out Fleet_ShipView shipView)
    {
        return spaceshipViews.TryGetValue(coords,out shipView );
    }
    public bool IsTileOccupied(Vector2Int coords)
    {
        return spaceshipViews.ContainsKey(coords);
    }
    private void ClearGrid()
    {
        Clear(gridRoot);
        Clear(offGridRoot);

        tileViews.Clear();
    }
    private void ClearShips()
    {
        Clear(shipsRoot);
        spaceshipViews.Clear();
    }
    private void Clear(Transform root)
    {
        foreach (Transform child in root)
            Destroy(child.gameObject);
    }
}
