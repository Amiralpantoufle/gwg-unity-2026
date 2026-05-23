using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    /* public void Load(GridLevel level, int id)
     {
         Debug.Log($"Load {level} ({id})");

         // TEMP MOCK API
         var map = FakeMap(level);


         OnGridLoaded?.Invoke(level, map);
     }*/
    public async Task Load(GridLevel level, int id)
    {
        Debug.Log($"Load {level} ({id})");

        GridMapResponse map = await LoadMapFromData(level, id);
        // TEMP MOCK API
        //var map = FakeMap(level);

        if (map == null)
        {
            Debug.LogError("Failed to load map");
            return;
        }

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
    private async Task<GridMapResponse> LoadMapFromData(GridLevel level, int id)
    {
        string json = await API_Client.Instance.GetAsync($"/map/planet/{id}");

        if (string.IsNullOrEmpty(json))
            return null;

        ApiResponse<GridAPIModels> response =
            JsonConvert.DeserializeObject<ApiResponse<GridAPIModels>>(json);

        if (response == null || response.error)
        {
            Debug.LogError("API ERROR");
            return null;
        }

        return ConvertToGridMapResponse(level, response.output);
    }
    /// <summary>
    /// Mapper API
    /// </summary>
    /// <param name="level"></param>
    /// <param name="apiData"></param>
    /// <returns></returns>
    private GridMapResponse ConvertToGridMapResponse(GridLevel level, GridAPIModels apiData)
    {
        GridMapResponse map = new GridMapResponse();

        map.level = level.ToString().ToLower();

        map.tiles = new List<GridTileDto>();

        foreach (var tile in apiData.tiles)
        {
            map.tiles.Add(new GridTileDto
            {
                x = tile.x,
                y = tile.y,
                entity_id = tile.entity_id,
                entities = tile.entities
            });
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
        //Get Player and Global Data
        BootStrap_Loader.Instance.Init_BootStrap();
    }
}