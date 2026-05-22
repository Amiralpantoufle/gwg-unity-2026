using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public event Action<GridLevel, GridMapResponse> OnGridLoaded;

    private void Awake()
    {
        Instance = this;
        GetComponent<GridRenderer>().Init_GridRenderer();
    }

    // =========================
    // ENTRY POINT
    // =========================

    public void Load(GridLevel level, int id)
    {
        Debug.Log($"Load {level} ({id})");

        // TEMP MOCK API
        var map = FakeMap(level);

        OnGridLoaded?.Invoke(level, map);
    }

    // =========================
    // MOCK
    // =========================

    private GridMapResponse FakeMap(GridLevel level)
    {
        var map = new GridMapResponse();
        map.level = level.ToString().ToLower();
        map.tiles = new System.Collections.Generic.List<GridTileDto>();

        int size = level switch
        {
            GridLevel.Galaxy => 4,
            GridLevel.SolarSystem => 6,
            GridLevel.Planet => 8,
            _ => 5
        };

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                map.tiles.Add(new GridTileDto
                {
                    x = x,
                    y = y,
                    entity_id = x * 100 + y,
                    entities = new System.Collections.Generic.List<EntityDto>()
                });
            }
        }

        return map;
    }


    //Inputs
    private void OnEnable()
    {
        MainView_Screen.OnMainViewLoaded += HandleMainViewLoaded;
    }
    private void OnDisable()
    {
        MainView_Screen.OnMainViewLoaded -= HandleMainViewLoaded;
    }
    private void HandleMainViewLoaded()
    {
        Load(GridLevel.Galaxy, 0);

    }
}